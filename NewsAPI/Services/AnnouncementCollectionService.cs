﻿using MongoDB.Driver;
using NewsAPI.Models;
using NewsAPI.Services;
using NewsAPI.Settings;

namespace NewsAPI.Controllers
{
    //implementeaza functile din interfata
    public class AnnouncementCollectionService : IAnnouncementCollectionService
    {
        //ia collection din baza de date (fiecare tabel)
        private readonly IMongoCollection<Announcement> _announcements;

        public AnnouncementCollectionService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _announcements = database.GetCollection<Announcement>(settings.AnnouncementsCollectionName);
        }
        public async Task<List<Announcement>> GetAll()
        {
            var result = await _announcements.FindAsync(announcement => true);
            return result.ToList();
        }
        public async Task<bool> Create(Announcement announcement)
        {
            if (announcement.Id == Guid.Empty)
            {
                announcement.Id = Guid.NewGuid();
            }

            await _announcements.InsertOneAsync(announcement);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _announcements.DeleteOneAsync(announcement => announcement.Id == id);
            if (!result.IsAcknowledged && result.DeletedCount == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<Announcement> Get(Guid id)
        {
            return (await _announcements.FindAsync(announcement => announcement.Id == id)).FirstOrDefault();
        }

        public async Task<bool> Update(Guid id, Announcement announcement)
        {
            announcement.Id = id;
            var result = await _announcements.ReplaceOneAsync(announcement => announcement.Id == id, announcement);
            if (!result.IsAcknowledged && result.ModifiedCount == 0)
            {
                await _announcements.InsertOneAsync(announcement);
                return false;
            }

            return true;
        }

        public async Task<List<Announcement>> GetAnnouncementsByCategoryId(string categoryId)
        {
            return (await _announcements.FindAsync(announcement => announcement.CategoryId == categoryId)).ToList();
        }


        //public List<Announcement> GetAll()
        //{
        //    return _announcements;
        //}
        //public Announcement Get(Guid id)
        //{
        //    throw new NotImplementedException();
        //}
        //public List<Announcement> GetAnnouncementsByCategoryId(string categoryId)
        //{
        //    throw new NotImplementedException();
        //}
        //public bool Update(Guid id, Announcement model)
        //{
        //    throw new NotImplementedException();
        //}
        //public bool Create(Announcement model)
        //{
        //    _announcements.Add(model);
        //    return true;
        //}

        //public bool Delete(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

    }
}