namespace MovistarPlus.Common.Dto
{
    public class ProfileDto
    {
        public ProfileDto (string profileName)
        {
            this.ProfileId = profileName;
        }
        public string ProfileId { get; set; }

		public override bool Equals(object obj)
		{
			return base.Equals(obj) || ((ProfileDto)obj).ProfileId == this.ProfileId;
		}
	}
}