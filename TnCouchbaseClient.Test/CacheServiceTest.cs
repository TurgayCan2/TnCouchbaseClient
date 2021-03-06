using System;
using Couchbase;
using Couchbase.IO;
using Xunit;

namespace TnCouchbaseClient.Test
{
    public class CacheServiceTest : IDisposable
    {
        private static readonly ICacheService cacheService = new CacheService();

        [Fact]
        public void should_add_object_to_cache()
        {
            bool isAdded = cacheService.Add("test1", "turgay");

            Assert.True(isAdded);
        }

        [Fact]
        public void should_not_add_object_to_cache_if_key_exists()
        {
            cacheService.Add("test1", "turgay");

            bool isAdded = cacheService.Add("test1", "turgay");

            Assert.False(isAdded);
        }

        [Fact]
        public void should_get_object_to_cache_if_key_exists()
        {
            CouchbaseCacheModel model = new CouchbaseCacheModel(1, "turgay");

            cacheService.Put<CouchbaseCacheModel>("test1", model);

            CouchbaseCacheModel cachedObjectValue = cacheService.Get<CouchbaseCacheModel>("test1");

            Assert.Equal(cachedObjectValue.Id, model.Id);
            Assert.Equal(cachedObjectValue.Name, model.Name);
        }

        [Fact]
        public void should_get_object_from_cache()
        {
            cacheService.Add("test1", "turgay");

            string cachedObjectValue = cacheService.GetAsValue<string>("test1");

            Assert.Equal("turgay", cachedObjectValue);
        }

        [Fact]
        public void should_put_object_cache_with_existing_one()
        {
            cacheService.Add("test1", "turgay");

            string cachedObjectValue = cacheService.Put<string>("test1", "turgay2");

            Assert.Equal("turgay2", cachedObjectValue);
        }

        [Fact]
        public void should_put_object_cache_if_not_exists()
        {
            string cachedObjectValue = cacheService.Put<string>("test1", "turgay3");

            Assert.Equal("turgay3", cachedObjectValue);
        }


        [Fact]
        public void should_remove_cache_object_if_exists()
        {
            cacheService.Add("test1", "turgay");

            bool isRemoved = cacheService.Remove("test1");

            Assert.True(isRemoved);
        }

        [Fact]
        public void should_not_remove_cache_object_if_key_not_exists()
        {
            bool isRemoved = cacheService.Remove("test1");

            Assert.False(isRemoved);
        }

        [Fact]
        public void should_remove_safely_cache_object()
        {
            cacheService.Add("test1", "turgay");

            bool isRemovedSafely = cacheService.RemoveSafely("test1");

            Assert.True(isRemovedSafely);
        }

        [Fact]
        public void should_return_one_if_many_attempt_to_fail_when_more_than_one_try_to_pass()
        {
            int maxAttempts = 10;
            int attempts = 0;
            int incremented = 0;

            do
            {
               bool isIncrement = cacheService.Increment("test1", TimeSpan.FromSeconds(10));

                if (isIncrement)
                {
                    ++incremented;
                }

            } while (attempts++ < maxAttempts);
            
            Assert.Equal(1, incremented);

        }




        public void Dispose()
        {
            cacheService.RemoveSafely("test1");
        }
    }
}
