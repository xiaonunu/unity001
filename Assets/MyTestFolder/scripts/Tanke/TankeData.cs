using UnityEngine;
using System.Collections;

public class TankeData
{
	//config--class...
	//ball
	private static int maxHp_ball = 100;
	private static int bulletNumsUp_ball = 0;
	private static float maxHp_up_ball = 0.2f;
	private static string name_ball="ball";
	private static string info_ball="tanke is ball";
	//bow
	private static int maxHp_bow = 100;
	private static float maxHp_up_bow = 0.1f;
	private static int bulletNumsUp_bow = 0;
	private static string name_bow="bow";
	private static string info_bow="tanke is bow";
	public static int maxLv = 10;

	public static int getMaxHp (int id)
	{
		if (id == 0) {
			return maxHp_ball;
		}
		if (id == 1) {
			return maxHp_bow;
		}
		return maxHp_ball;
	}

	public static float getMaxHpUp (int id)
	{
		if (id == 0) {
			return maxHp_up_ball;
		}
		if (id == 1) {
			return maxHp_up_bow;
		}
		return maxHp_up_ball;
	}
		public static string getName (int id)
	{
		if (id == 0) {
			return name_ball;
		}
		if (id == 1) {
			return name_bow;
		}
		return name_ball;
	}
		public static string getInfo (int id)
	{
		if (id == 0) {
			return info_ball;
		}
		if (id == 1) {
			return info_bow;
		}
		return info_ball;
	}

	public static int getBulletNumsUp (int id)
	{
		if (id == 0) {
			return bulletNumsUp_ball;
		}
		if (id == 1) {
			return bulletNumsUp_bow;
		}
		return bulletNumsUp_ball;
	}
}
