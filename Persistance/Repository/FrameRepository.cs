﻿using DataViewerApi.Exception;
using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IFrameRepository
{
    Task<Frame> AddFrame(Frame frame);
    Task<Frame> GetFrameById(int id);
    Task UpdateFrameLap(int frameId, int lap);
    Task UpdateFrame(Frame frame);
    Task<IEnumerable<int>> GetFrameIdsByVideoId(int videoId);
    Task<List<Frame>> GetFrameByVideoIdAndTimestamps(int videoId, int initTime, int endTime);
}

public class FrameRepository : IFrameRepository
{
    private readonly ApplicationDbContext _db;

    public FrameRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Frame> AddFrame(Frame frame)
    {
        await _db.Frames.AddAsync(frame);
        await _db.SaveChangesAsync();
        return frame;
    }

    public async Task<Frame> GetFrameById(int id)
    {
        return await _db.Frames.FirstAsync(f => f.FrameId == id);
    }

    public async Task UpdateFrameLap(int frameId, int lap)
    {
        var frame = await GetFrameById(frameId);
        frame.Lap = lap;
        await UpdateFrame(frame);
    }

    public async Task UpdateFrame(Frame frame)
    {
        _db.Frames.Update(frame);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<int>> GetFrameIdsByVideoId(int videoId)
    {
        return await _db.Frames.Where(f => f.VideoId == videoId).Select(f => f.FrameId).ToListAsync();
    }

    public async Task<List<Frame>> GetFrameByVideoIdAndTimestamps(int videoId, int initTime, int endTime)
    {
        var frames = await _db.Frames
            .Where(f => f.VideoId == videoId && f.Timestamp >= initTime && f.Timestamp <= endTime)
            .ToListAsync();
        if (frames == null || frames.Count == 0)
        {
            throw new FrameNotFoundException($"Frame not found with videoId: {videoId} and timestamp between {initTime} and {endTime}");
        }
        return frames;
    }
}