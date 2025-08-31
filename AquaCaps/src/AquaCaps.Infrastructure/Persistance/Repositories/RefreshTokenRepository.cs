using AquaCaps.Application.Interface;
using AquaCaps.Core.Errors;
using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Infrastructure.Persistance.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;
    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task InsertRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        if (refreshToken == null)
        {
            throw new EntityNotFoundException("Refresh token not found");
        }
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken> SelectRefreshTokenAsync(string refreshToken, long userId)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken.Token);
        if (existingToken == null)
        {
            throw new EntityNotFoundException($"Refresh token {refreshToken.Token} not found for user {refreshToken.UserId}");
        }
        existingToken.IsRevoked = refreshToken.IsRevoked;

        existingToken.Expire = refreshToken.Expire;

        _context.RefreshTokens.Update(existingToken);
        await _context.SaveChangesAsync();
    }
}
