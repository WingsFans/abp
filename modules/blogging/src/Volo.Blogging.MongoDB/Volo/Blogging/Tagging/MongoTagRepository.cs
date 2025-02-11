﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.Blogging.Tagging
{
    public class MongoTagRepository : MongoDbRepository<IBloggingMongoDbContext, Tag, Guid>, ITagRepository
    {
        public MongoTagRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<Tag>> GetListAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync(cancellationToken)).Where(t => t.BlogId == blogId).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Tag> GetByNameAsync(Guid blogId, string name, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync(cancellationToken)).Where(t => t.BlogId == blogId && t.Name == name).FirstAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Tag> FindByNameAsync(Guid blogId, string name, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync(cancellationToken)).Where(t => t.BlogId == blogId && t.Name == name).FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync(cancellationToken)).Where(t => ids.Contains(t.Id)).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task DecreaseUsageCountOfTagsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var tags = await (await GetQueryableAsync(cancellationToken))
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var tag in tags)
            {
                tag.DecreaseUsageCount();
                await UpdateAsync(tag, cancellationToken: GetCancellationToken(cancellationToken));
            }
        }
    }
}
