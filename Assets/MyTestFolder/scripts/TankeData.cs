using UnityEngine;
using System.Collections;

public class TankeData
{
	//config--class...
	private static int radius_ball = 50;
	private static int attack_ball = 50;
	private static int maxHp_ball = 100;
	private static int bulletNumsUp_ball = 0;
	private static float radius_up_ball = 0.2f;
	private static float attack_up_ball = 0.1f;
	private static float maxHp_up_ball = 0.2f;
	
	private static int radius_bow = 50;
	private static int attack_bow = 50;
	private static int maxHp_bow = 100;
	private static float radius_up_bow = 0.1f;
	private static float attack_up_bow = 0.2f;
	private static float maxHp_up_bow = 0.1f;
	private static int bulletNumsUp_bow = 0;
	
	public static int maxLv = 10;
	
	public static float getRadiusUp (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return radius_up_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return radius_up_bow;
		}
		return radius_up_ball;
	}

	public static int getRadius (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return radius_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return radius_bow;
		}
		return radius_ball;
	}

	public static int getMaxHp (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return maxHp_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return maxHp_bow;
		}
		return maxHp_ball;
	}

	public static float getMaxHpUp (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return maxHp_up_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return maxHp_up_bow;
		}
		return maxHp_up_ball;
	}

	public static int getAttack (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return attack_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return attack_bow;
		}
		return attack_ball;
	}

	public static float getAttackUp (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return attack_up_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return attack_up_bow;
		}
		return attack_up_ball;
	}

	public static int getBulletNumsUp (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return bulletNumsUp_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return bulletNumsUp_bow;
		}
		return bulletNumsUp_ball;
	}
}
