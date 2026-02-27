using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ShadowRace.Player;

namespace ShadowRace.Tests
{
    public class PlayerTests
    {
        private GameObject playerObject;
        private PlayerStats playerStats;

        [SetUp]
        public void Setup()
        {
            // This runs before every test
            playerObject = new GameObject("Player");
            playerStats = playerObject.AddComponent<PlayerStats>();
            
            // Initialize basic stats
            playerStats.maxHP = 100f;
            playerStats.currentHP = 100f;
            playerStats.defense = 10f;
        }

        [TearDown]
        public void Teardown()
        {
            // This runs after every test
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void PlayerTakesDamage_CorrectlyReducesHealth()
        {
            // Arrange
            float incomingDamage = 30f;
            float expectedHealth = 100f - (30f - 10f); // 100 - (Damage - Defense) = 80

            // Act
            playerStats.TakeDamage(incomingDamage);

            // Assert
            Assert.AreEqual(expectedHealth, playerStats.currentHP, "HP did not reduce correctly based on Defense formula.");
        }

        [Test]
        public void PlayerTakeLethalDamage_HealthDoesNotDropBelowZero()
        {
            // Arrange
            float incomingDamage = 200f; // More than maxHP + defense

            // Act
            playerStats.TakeDamage(incomingDamage);

            // Assert
            Assert.AreEqual(0f, playerStats.currentHP, "HP dropped below zero.");
        }

        [Test]
        public void PlayerHeal_DoesNotExceedMaxHealth()
        {
            // Arrange
            playerStats.currentHP = 50f;

            // Act
            playerStats.Heal(100f);

            // Assert
            Assert.AreEqual(playerStats.maxHP, playerStats.currentHP, "Healing exceeded max HP limit.");
        }
        
        [Test]
        public void PlayerLevelUp_IncreasesMaxHealth()
        {
            // Arrange
            float initialMaxHP = playerStats.maxHP;
            
            // Act
            playerStats.AddXP(playerStats.xpToNextLevel); // Assuming this triggers LevelUp

            // Assert
            Assert.Greater(playerStats.maxHP, initialMaxHP, "Max HP did not increase upon leveling up.");
        }
    }
}
