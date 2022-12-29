namespace SmartGloveRebuild2.Models.Group
{
    public class CreateGroupDTO
    {
        public string GroupName { get; set; }
        private readonly StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        public override bool Equals(object other)
        {
            //This casts the object to null if it is not a Account and calls the other Equals implementation.
            return this.Equals(other as CreateGroupDTO);
        }

        public override int GetHashCode()
        {
            return comparer.GetHashCode(this.GroupName);
        }

        public bool Equals(CreateGroupDTO other)
        {
            if (other == null)
                return false;

            return comparer.Equals(this.GroupName, other.GroupName);
        }
    }
}
