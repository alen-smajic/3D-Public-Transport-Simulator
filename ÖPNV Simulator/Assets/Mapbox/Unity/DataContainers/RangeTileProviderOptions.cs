namespace Mapbox.Unity.Map
{
	using System;
	using UnityEngine;

	[Serializable]
	public class RangeTileProviderOptions : ExtentOptions
	{
        [Range(0, 10)]
		public int west = OsmBounds.LenghtFactor;
		[Range(0, 10)]
		public int north = OsmBounds.WidthFactor;
		[Range(0, 10)]
		public int east = OsmBounds.LenghtFactor;
		[Range(0, 10)]
		public int south = OsmBounds.WidthFactor;

		public override void SetOptions(ExtentOptions extentOptions)
		{
			RangeTileProviderOptions options = extentOptions as RangeTileProviderOptions;
			if (options != null)
			{
                west = options.west;
                north = options.north;
                east = options.east;
                south = options.south;
			}
			else
			{
				Debug.LogError("ExtentOptions type mismatch : Using " + extentOptions.GetType() + " to set extent of type " + this.GetType());
			}
		}

		public void SetOptions(int northRange = 1, int southRange = 1, int eastRange = 1, int westRange = 1)
		{
			west = westRange;
			north = northRange;
			east = eastRange;
			south = southRange;
		}
	}
}
