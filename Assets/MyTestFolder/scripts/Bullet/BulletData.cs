using UnityEngine;
using System.Collections;

public class BulletData
{
	//ball
	private static int radius_ball = 50;
	private static int attack_ball = 50;
	private static float radius_up_ball = 0.2f;
	private static float attack_up_ball = 0.1f;
	//bow
	private static int radius_bow = 50;
	private static int attack_bow = 50;
	private static float radius_up_bow = 0.1f;
	private static float attack_up_bow = 0.2f;

	public static float getRadiusUp (int id)
	{
		if (id == 0) {
			return radius_up_ball;
		}
		if (id == 1) {
			return radius_up_bow;
		}
		return radius_up_ball;
	}

	public static int getRadius (int id)
	{
		if (id == 0) {
			return radius_ball;
		}
		if (id == 1) {
			return radius_bow;
		}
		return radius_ball;
	}

	public static int getAttack (int id)
	{
		if (id == 0) {
			return attack_ball;
		}
		if (id == 1) {
			return attack_bow;
		}
		return attack_ball;
	}

	public static float getAttackUp (int id)
	{
		if (id == 0) {
			return attack_up_ball;
		}
		if (id == 1) {
			return attack_up_bow;
		}
		return attack_up_ball;
	}
}
