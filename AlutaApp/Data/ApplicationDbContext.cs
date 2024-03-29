﻿using AlutaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlutaApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<BannerAd> BannerAds { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }
        public DbSet<CGPA> CGPAs { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<ChatGroupMessage> GroupMessages { get; set; }
        public DbSet<ChatGroupUser> ChatGroupUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentCategory> DocumentCategories { get; set; }
        public DbSet<DocumentComment> DocumentComments { get; set; }
        public DbSet<DocumentCommentLike> DocumentCommentLikes { get; set; }
        public DbSet<DocumentLike> DocumentLikes { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<HubConnection> HubConnections { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PhoneNumberConfirmation> PhoneNumberConfirmations { get; set; }
        public DbSet<Points2Earn> Points2Earns { get; set; }
        //public DbSet<PointsLog> PointsLogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionPayments> PromotionPayments { get; set; }
        public DbSet<ReportedComment> ReportedComments { get; set; }
        public DbSet<ReportedPost> ReportedPosts { get; set; }
        public DbSet<ReportedUser> ReportedUsers { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<SmsVerification> SmsVerifications { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<StatusView> StatusViews { get; set; }
        public DbSet<TGIFMatch> TGIFMatches { get; set; }
        public DbSet<TGIFMessage> TGIFMessages { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<Trivia> Trivias { get; set; }
        public DbSet<TriviaAttempt> TriviaAttempts { get; set; }
        public DbSet<TriviaQuestion> TriviaQuestions { get; set; }
        public DbSet<TriviaResult> TriviaResults { get; set; }
        public DbSet<TriviaWinner> TriviaWinners { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }

    }
}