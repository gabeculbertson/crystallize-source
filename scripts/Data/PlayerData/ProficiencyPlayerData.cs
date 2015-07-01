using UnityEngine;
using System.Collections;

public class ProficiencyPlayerData {

    public static int GetReviewExperienceForLevel(int level){
        if(level == 1){
            return 0;
        } else if(level == 2){
            return 5;
        } else if(level == 3){
            return 10;
        } else if(level == 4){
            return 20;
        } else if(level == 5){
            return 40;
        } else if(level == 6){
            return 60;
        } else {
            return (level - 6) * 20 + 80;
        }
    }

    public int ReviewExperience { get; set; }
    public int Phrases { get; set; }
    public int Words { get; set; }

    public ProficiencyPlayerData() {
        Phrases = 0;
        Words = 1;
    }

    public int GetReviewLevel() {
        int level = 1;
        int xp = ReviewExperience;
        while (GetReviewExperienceForLevel(level + 1) < xp) {
            level++;
            xp -= GetReviewExperienceForLevel(level);
        }
        return level;
    }

    public int GetReviewLevelExperience() {
        int level = 1;
        int xp = ReviewExperience;
        while (GetReviewExperienceForLevel(level + 1) < xp) {
            level++;
            xp -= GetReviewExperienceForLevel(level);
        }
        return xp;
    }

    public void SetParametersForLevel() {
        var l = GetReviewLevel();
        if (l == 1) {
            Phrases = 0;
            Words = 1;
        } else if (l == 2) {
            Phrases = 1;
            Words = 1;
        } else if (l == 3) {
            Phrases = 1;
            Words = 2;
        } else if (l == 4) {
            Phrases = 1;
            Words = 3;
        } else if (l == 5) {
            Phrases = 2;
            Words = 3;
        } else {
            Phrases = 2 + (l - 5) / 2;
            Words = 3 + (l - 4) / 2; ;
        }
    }

}