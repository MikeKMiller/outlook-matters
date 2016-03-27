﻿namespace OutlookMatters.Mattermost.Session
{
    public interface ISession
    {
        void CreatePost(string channelId, string message, string rootId = "");
        Post GetPostById(string postId);
    }
}