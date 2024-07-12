﻿namespace Common.Core.Entities
{
    public class LGA : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int StateId { get; set; }
        public State? State { get; set; }
    }
}
