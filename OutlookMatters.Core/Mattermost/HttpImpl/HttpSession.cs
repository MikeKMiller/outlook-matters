﻿using System;
using System.Collections.Generic;
using System.Linq;
using OutlookMatters.Core.Mattermost.Interface;

namespace OutlookMatters.Core.Mattermost.HttpImpl
{
    public class HttpSession : ISession
    {
        private readonly Uri _baseUri;
        private readonly IRestService _restService;
        private readonly string _token;
        private readonly string _userId;
        private readonly IChatPostFactory _factory;
        private readonly IChatChannelFactory _channelFactory;

        public HttpSession(IRestService restService, Uri baseUri, string token, string userId, IChatPostFactory factory,
            IChatChannelFactory channelFactory)
        {
            _baseUri = baseUri;
            _token = token;
            _userId = userId;
            _restService = restService;
            _factory = factory;
            _channelFactory = channelFactory;
        }

        public void CreatePost(string channelId, string message)
        {
            var post = new Post
            {
                Id = string.Empty,
                ChannelId = channelId,
                Message = message,
                UserId = _userId,
                RootId = string.Empty
            };
            _restService.CreatePost(_baseUri, _token, channelId, post);
        }

        public IChatChannel GetChannel(string channelId)
        {
            return _channelFactory.NewInstance(_restService, _baseUri, _token, _userId,
                new Channel {ChannelId = channelId});
        }

        public IChatPost GetPost(string postId)
        {
            var thread = _restService.GetThreadOfPosts(_baseUri, _token, postId);
            return _factory.NewInstance(_baseUri, _token, _userId, thread.Posts[postId]);
        }

        public ChannelList FetchChannelList()
        {
            return _restService.GetChannelList(_baseUri, _token);
        }

        public IEnumerable<IChatChannel> GetChannels()
        {
            return _restService.GetChannelList(_baseUri, _token).Channels
                .Select(c =>
                    _channelFactory.NewInstance(_restService, _baseUri, _token, _userId, c));
        }
    }
}