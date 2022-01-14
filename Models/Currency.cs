using StocksStand.Models.Base;

namespace StocksStand.Models
{
	public class Currency : Entity
	{
		private string _Fullname;
		public string Fullname
		{
			get => _Fullname;
			set => Set(ref _Fullname, value);
		}

		private string _Shortname;
		public string Shortname
		{
			get => _Shortname;
			set => Set(ref _Shortname, value);
		}

		private char _Symbol;
		public char Symbol
		{
			get => _Symbol;
			set => Set(ref _Symbol, value);
		}
	}
}
