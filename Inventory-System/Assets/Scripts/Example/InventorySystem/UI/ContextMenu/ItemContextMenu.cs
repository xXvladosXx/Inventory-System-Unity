﻿using System.Collections.Generic;
using DG.Tweening;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using InventorySystem.UI.ClickAction;
using UnityEngine;

namespace InventorySystem.UI.ContextMenu
{
    public class ItemContextMenu : MonoBehaviour
    {
        [SerializeField] private ItemContextOption _itemContextOptionPrefab;
        [SerializeField] private float _menuAppearDuration = 0.2f;
        [SerializeField] private float _itemAppearInterval = 0.1f;
        
        private readonly List<ItemContextOption> _activeOptions = new List<ItemContextOption>();
        private readonly Queue<ItemContextOption> _pool = new Queue<ItemContextOption>();

        public IReadOnlyList<ItemContextOption> ActiveOptions => _activeOptions;

        private Tween _menuTween;

        private void Awake()
        {
            Hide();
        }
        
        public void Show(List<ItemClickAction> actions, Vector3 slotPosition)
        {
            Hide();

            gameObject.SetActive(true);
            transform.position = slotPosition;
            transform.localScale = Vector3.zero;
            
            _menuTween?.Kill();
            _menuTween = transform.DOScale(1, _menuAppearDuration).SetEase(Ease.OutBack);
            int index = 0;

            foreach (var action in actions)
            {
                ItemContextOption option = GetOrCreateOption();
                option.SetOption(action);
                option.transform.localScale = Vector3.zero;

                option.transform.SetSiblingIndex(index);
                option.gameObject.SetActive(true);
                _activeOptions.Add(option);

                option.KillTweens();
                option.AnimateAppearance(_menuAppearDuration, _itemAppearInterval * index);
                index++;
            }
        }

        public void Hide()
        {
            _menuTween?.Kill();

            foreach (var option in _activeOptions)
            {
                option.KillTweens();
                option.gameObject.SetActive(false);
                _pool.Enqueue(option);
            }
            
            _activeOptions.Clear();
            gameObject.SetActive(false);
        }

        private ItemContextOption GetOrCreateOption() => 
            _pool.Count > 0 ? _pool.Dequeue() : Instantiate(_itemContextOptionPrefab, transform);
    }
}