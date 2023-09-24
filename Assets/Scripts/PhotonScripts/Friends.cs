namespace PhotonScripts
{
    public class Friends
    {
        private readonly string _userName;
        private readonly string _description;
        private readonly string _userID;

        public string UserName => _userName;

        public string Description => _description;

        public string UserID => _userID;

        public Friends(string uName, string description, string uID)
        {
            _userName = uName;
            _description = description;
            _userID = uID;
        }
    }
}