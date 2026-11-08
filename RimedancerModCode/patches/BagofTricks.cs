using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace RimedancerMod.RimedancerModCode;

public class BagofTricks : CustomPile
{
    [CustomEnum]
    public static PileType bagoftricks;
    public BagofTricks() : base(bagoftricks)
    {
        
    }

    public override bool CardShouldBeVisible(CardModel card)
    {
        return false;
    }

    public override Vector2 GetTargetPosition(CardModel model, Vector2 size)
    {
        return Vector2.Zero;
    }
    
 public static async Task<CardModel?> Draw(PlayerChoiceContext choiceContext, Player player)
  {
    return (await Flourish(choiceContext, 1M, player)).FirstOrDefault<CardModel>();
  }

  /// <summary>Draw cards.</summary>
  /// <param name="choiceContext">The context with which to handle player choices.</param>
  /// <param name="count">Number of cards to draw.</param>
  /// <param name="player">Player who the hand and draw pile belongs to.</param>
  /// <param name="fromHandDraw">If this draw happened as part of the initial card draws at the start of your turn.</param>
  /// <returns>Cards that were drawn.</returns>
  public static async Task<IEnumerable<CardModel>> Flourish(
    PlayerChoiceContext choiceContext,
    Decimal count,
    Player player)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    AbstractModel modifier;
    ICombatState combatState = player.Creature.CombatState;
    List<CardModel> result = new List<CardModel>();
    CardPile hand = PileType.Hand.GetPile(player);
    CardPile BagofTricksPile = bagoftricks.GetPile(player);
    int drawsRequested = count > 0M ? (int) Math.Ceiling(count) : 0;
    if (drawsRequested == 0)
      return (IEnumerable<CardModel>) result;
    int num = Math.Max(0, CardPile.MaxCardsInHand - hand.Cards.Count);
    if (num == 0)
    {
      CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player);
      return (IEnumerable<CardModel>) result;
    }
    for (int i = 0; i < drawsRequested && num > 0 && !CombatManager.Instance.IsOverOrEnding && CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player); ++i)
    {
      await ShuffleIfNecessary(choiceContext, player);
      if (CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player))
      {
        CardModel card = BagofTricksPile.Cards.FirstOrDefault<CardModel>();
        if (card != null && hand.Cards.Count < CardPile.MaxCardsInHand)
        {
          result.Add(card);
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, hand);
          CombatManager.Instance.History.CardDrawn(combatState, card, false);
          await Hook.AfterCardDrawn(combatState, choiceContext, card, false);
          card.InvokeDrawn();
          NDebugAudioManager.Instance?.Play("card_deal.mp3", 0.25f, PitchVariance.Small);
          num = Math.Max(0, CardPile.MaxCardsInHand - hand.Cards.Count);
          card = (CardModel) null;
        }
        else
          break;
      }
      else
        break;
    }
    return (IEnumerable<CardModel>) result;
  }
  private static bool CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(Player player)
  {
    int Spellcount = 0;
    foreach (var card in PileType.Draw.GetPile(player).Cards)
    {
      if (card.CanonicalKeywords.Contains(MainFile.Spell))
      {
        Spellcount++;
      }
    }
    foreach (var card in PileType.Discard.GetPile(player).Cards)
    {
      if (card.CanonicalKeywords.Contains(MainFile.Spell))
      {
        Spellcount++;
      }
    }
    foreach (var card in PileType.Exhaust.GetPile(player).Cards)
    {
      if (card.CanonicalKeywords.Contains(MainFile.Spell))
      {
        Spellcount++;
      }
    }
    if (bagoftricks.GetPile(player).Cards.Count + Spellcount == 0)
    {
      ThinkCmd.Play(new LocString("combat_messages", "NO_DRAW"), player.Creature, 2.0);
      return false;
    }
    if (bagoftricks.GetPile(player).Cards.Count < CardPile.MaxCardsInHand)
      return true;
    ThinkCmd.Play(new LocString("combat_messages", "HAND_FULL"), player.Creature, 2.0);
    return false;
  }
    public static async Task Shuffle(PlayerChoiceContext choiceContext, Player player)
  {
    CardPile BagofTricksPile = bagoftricks.GetPile(player);
    HashSet<CardModel> BagofTricksCards;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      BagofTricksPile = (CardPile) null;
      BagofTricksCards = (HashSet<CardModel>) null;
    }
    else
    {
      BagofTricksPile = bagoftricks.GetPile(player);
      List<CardModel> list = new List<CardModel>();
      float timeBetweenCardAdds = Mathf.Min(0.045f, 0.8f / (float) list.Count);
      float randomTimeBetweenCardAdds = 1.11f * timeBetweenCardAdds;
      BagofTricksCards = bagoftricks.GetPile(player).Cards.ToHashSet<CardModel>();
      foreach (var card in PileType.Draw.GetPile(player).Cards)
      {
        if (card.CanonicalKeywords.Contains(MainFile.Spell))
        {
          list.Add(card);
        }
      }
      foreach (var card in PileType.Discard.GetPile(player).Cards)
      {
        if (card.CanonicalKeywords.Contains(MainFile.Spell))
        {
          list.Add(card);
        }
      }
      foreach (var card in PileType.Exhaust.GetPile(player).Cards)
      {
        if (card.CanonicalKeywords.Contains(MainFile.Spell))
        {
          list.Add(card);
        }
      }
      list.AddRange((IEnumerable<CardModel>)  BagofTricksCards);
      list.StableShuffle<CardModel>(player.RunState.Rng.Shuffle);
      Hook.ModifyShuffleOrder(player.Creature.CombatState, player, list, false);
      foreach (CardModel card in  BagofTricksCards)
        BagofTricksPile.RemoveInternal(card, true);
      if (CombatManager.Instance.DebugForcedTopCardOnNextShuffle != null)
      {
        if (!list.Remove(CombatManager.Instance.DebugForcedTopCardOnNextShuffle))
          throw new InvalidOperationException($"Could not find card {CombatManager.Instance.DebugForcedTopCardOnNextShuffle.Id.Entry} in discard pile.");
        list.Insert(0, CombatManager.Instance.DebugForcedTopCardOnNextShuffle);
        CombatManager.Instance.DebugClearForcedTopCardOnNextShuffle();
      }
      float waitTimeAccumulator = 0.0f;
      foreach (CardModel card in list)
      {
        if (! BagofTricksCards.Contains(card))
        {
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, BagofTricksPile);
          if (CombatManager.Instance.IsOverOrEnding)
          {
             BagofTricksPile = (CardPile) null;
             BagofTricksCards = (HashSet<CardModel>) null;
            return;
          }
          float seconds = timeBetweenCardAdds + Rng.Chaotic.NextFloat((float) (-(double) randomTimeBetweenCardAdds * 0.5), randomTimeBetweenCardAdds * 0.5f);
          waitTimeAccumulator += seconds;
          if ((double) waitTimeAccumulator >= ((SceneTree) Engine.GetMainLoop()).Root.GetProcessDeltaTime())
          {
            await Cmd.Wait(seconds);
            waitTimeAccumulator = 0.0f;
          }
        }
        else
          BagofTricksPile.AddInternal(card, silent: true);
      }
      await Cmd.CustomScaledWait(0.2f, 0.5f);
      bagoftricks.GetPile(player).Cards.ToList<CardModel>().StableShuffle<CardModel>(player.RunState.Rng.CombatCardSelection);
      if (CombatManager.Instance.IsOverOrEnding)
      {
         BagofTricksPile = (CardPile) null;
         BagofTricksCards = (HashSet<CardModel>) null;
      }
      else
      {
         BagofTricksPile = (CardPile) null;
         BagofTricksCards = (HashSet<CardModel>) null;
      }
    }
  }
  public static async Task ShuffleIfNecessary(PlayerChoiceContext choiceContext, Player player)
  {
    CardPile pile1 = bagoftricks.GetPile(player);
    int Spellcount = 0;
    foreach (var card in PileType.Draw.GetPile(player).Cards)
    {
      if (card.CanonicalKeywords.Contains(MainFile.Spell))
      {
        Spellcount++;
      }
    }
    foreach (var card in PileType.Discard.GetPile(player).Cards)
    {
      if (card.CanonicalKeywords.Contains(MainFile.Spell))
      {
        Spellcount++;
      }
    }
    foreach (var card in PileType.Exhaust.GetPile(player).Cards)
    {
      if (card.CanonicalKeywords.Contains(MainFile.Spell))
      {
        Spellcount++;
      }
    }

    if (pile1.Cards.Any<CardModel>() || Spellcount < 1)
    {
      bagoftricks.GetPile(player).Cards.ToList<CardModel>().StableShuffle<CardModel>(player.RunState.Rng.CombatCardSelection);
      return;
    } await Shuffle(choiceContext, player);
  }
}