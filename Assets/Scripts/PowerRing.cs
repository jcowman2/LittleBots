using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRing : MonoBehaviour {

    public float timeToDestroy;

    [ReadOnly]
    public float degenerationTime;

    [ReadOnly]
    public string state;

    [ReadOnly]
    public int botsThrough;

    [ReadOnly]
    public List<LinkBehavior> occuipiedBots;

    [ReadOnly]
    public List<LinkBehavior> botHistory;

    private GameControl game;

	void Start () {
        game = GameObject.FindGameObjectWithTag(R.GAME_CONTROLLER).GetComponent<GameControl>();
        state = R.UNTOUCHED;
	}
	
	void Update () {
        if (state == R.DEGENERATING) {
            degenerationTime += Time.deltaTime;

            if (degenerationTime >= timeToDestroy) {
                DestroyRing();
            }
        }
	}

    private void OnTriggerEnter2D (Collider2D collision) {
        LinkBehavior bot = collision.GetComponent<LinkBehavior>();
        if (bot != null && bot.state != R.NEWBORN) {
            OnBotEnter(bot);
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        LinkBehavior bot = collision.GetComponent<LinkBehavior>();
        if (bot != null && bot.state != R.NEWBORN) {
            OnBotExit(bot);
        }
    }

    void OnBotEnter(LinkBehavior bot) {
        if (!botHistory.Contains(bot)) {
            occuipiedBots.Add(bot);
            botHistory.Add(bot);

            state = R.OCCUPIED;
            degenerationTime = 0;
            botsThrough++;

            game.changeChargeLevel(bot.scorePoints);
        }
    }

    void OnBotExit(LinkBehavior bot) {
        occuipiedBots.Remove(bot);
        if (occuipiedBots.Count == 0) {
            state = R.DEGENERATING;
        }
    }

    void DestroyRing() {
        gameObject.SetActive(false);
    }
}
