using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Infrastructure.Persistence;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly TechSalesDbContext _dbContext;

    public ReviewRepository(TechSalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddReviewAsync(Review review)
    {
        var dbModel = new ReviewDbModel
        {
            id = Guid.NewGuid(),
            user_id = review.userId,
            product_id = review.productId,
            rating = review.rating,
            comment = review.comment,
            status = ReviewStatus.VISIBLE,
            created_at = DateTimeOffset.UtcNow,
            updated_at = DateTimeOffset.UtcNow
        };

        await _dbContext.Reviews.AddAsync(dbModel);
    }

    public async Task<(List<Review> reviews, int totalCount, decimal averageRating)> GetReviewsByProductIdAsync(Guid productId, int pageNumber, int pageSize)
    {
        var query = _dbContext.Reviews
            .Include(r => r.user)
                .ThenInclude(u => u!.user_profile)
            .Where(r => r.product_id == productId && r.status == ReviewStatus.VISIBLE);

        int totalCount = await query.CountAsync();
        
        decimal averageRating = 0;
        if (totalCount > 0)
        {
            double avg = await query.AverageAsync(r => r.rating);
            // BR138: Rounded to 1 decimal place
            averageRating = Math.Round((decimal)avg, 1);
        }

        var dbReviews = await query
            .OrderByDescending(r => r.created_at)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var mappedReviews = dbReviews.Select(db => new Review
        {
            id = db.id,
            userId = db.user_id,
            productId = db.product_id,
            rating = db.rating,
            comment = db.comment,
            status = db.status ?? ReviewStatus.VISIBLE,
            createdAt = db.created_at ?? DateTimeOffset.UtcNow,
            updatedAt = db.updated_at,
            profile = new UserProfile
            {
                fullName = db.user?.user_profile?.full_name ?? db.user?.email ?? "Anonymous Customer",
                avatarUrl = db.user?.user_profile?.avatar_url,
                phone = db.user?.user_profile?.phone ?? string.Empty,
                dateOfBirth = db.user?.user_profile?.date_of_birth
            }
        }).ToList();

        return (mappedReviews, totalCount, averageRating);
    }
}
