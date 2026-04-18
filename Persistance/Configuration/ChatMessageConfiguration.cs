using Domain.Entitiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Messages");

            builder.HasOne(y => y.Sender).
                WithMany().
                HasForeignKey(x => x.SenderId).
                OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(y => y.Receiver).
               WithMany().
               HasForeignKey(x => x.ReceiverId).
               OnDelete(DeleteBehavior.Restrict);

            builder.Property(y => y.Content).IsRequired().HasMaxLength(250);


        }
    }
}
