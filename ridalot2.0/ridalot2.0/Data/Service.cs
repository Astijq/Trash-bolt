﻿using ridalot2._0.Data.RIDALOT;
using Microsoft.EntityFrameworkCore;

namespace ridalot2._0.Data
{
    public class Service
    {
        private Lazy<RIDALOTContext> _context;
        public RIDALOTContext context
        {
            get
            {
                return _context.Value;
            }
        }
        //private readonly RIDALOTContext _context;
        public Service(RIDALOTContext context)
        {
            _context = new Lazy<RIDALOTContext>(() => context);
        }
        public async Task<List<Posts>>
            GetAllPostsAsync()
        {
            return await _context.Value.Posts
                 .AsNoTracking().ToListAsync();
        }
        public async Task<List<Posts>>
            GetMyPostsAsync(string strCurrentUser)
        {
            return await _context.Value.Posts
                 .Where(x => x.User == strCurrentUser)
                 .AsNoTracking().ToListAsync();
        }
        public async Task<List<Images>>
            GetImagesAsync()
        {
            return await _context.Value.Images.Include(p => p.Posts)
                 .AsNoTracking().ToListAsync();
        }
        public async Task<List<Posts>>
            GetMyTasksAsync(string strCurrentUser)
        {
            return await _context.Value.Posts
                 .Where(x => x.Worker == strCurrentUser)
                 .AsNoTracking().ToListAsync();
        }
        public async Task<List<Posts>>
            GetFeedPostsAsync(Status strCurrentUser)
        {
            return await _context.Value.Posts
                 .Where(x => x.Status == strCurrentUser)
                 .AsNoTracking().ToListAsync();
        }
        public async Task<Posts>
            CreatePostAsync(Posts post)
        {
            _context.Value.Posts.Add(post);
            _context.Value.SaveChanges();
            return await Task.FromResult(post);
        }

        public async Task<Workers>
             CreateWorkerAsync(Workers worker)
        {
            _context.Value.Workers.Add(worker);
            _context.Value.SaveChanges();
            return await Task.FromResult(worker);
        }

        public async Task<Images>
             CreateImageAsync(Images img)
        {
            _context.Value.Images.Add(img);
            _context.Value.SaveChanges();
            return await Task.FromResult(img);
        }

        public Task<bool> //TODO
           DeletePostAsync(Posts post)
        {
            var ExistingPost =
                _context.Value.Posts
                .Where(x => x.Id == post.Id)
                .FirstOrDefault();
            if (ExistingPost != null)
            {
                _context.Value.Posts.Remove(ExistingPost);
                _context.Value.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<bool>
            UpdatePostAsync(Posts post)
        {
            var ExistingPost =
                _context.Value.Posts
                .Where(x => x.Id == post.Id)
                .FirstOrDefault();
            if (ExistingPost != null)
            {
                ExistingPost.Status =
                    post.Status;
                ExistingPost.Worker =
                    post.Worker;
                _context.Value.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public async Task<List<Workers>>
            GetAllWorkersAsync()
        {
            return await _context.Value.Workers
                 .AsNoTracking().ToListAsync();
        }

    }
}
