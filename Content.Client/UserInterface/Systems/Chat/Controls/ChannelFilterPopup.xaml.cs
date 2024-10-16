﻿using Content.Shared.Chat;
using Content.Shared.CCVar;
using Robust.Shared.Utility;
using Robust.Shared.Configuration;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using static Robust.Client.UserInterface.Controls.BaseButton;
using static Robust.Client.UserInterface.Controls.TextEdit;

namespace Content.Client.UserInterface.Systems.Chat.Controls;

[GenerateTypedNameReferences]
public sealed partial class ChannelFilterPopup : Popup
{
    // order in which the available channel filters show up when available
    private static readonly ChatChannel[] ChannelFilterOrder =
    {
        ChatChannel.Local,
        ChatChannel.Whisper,
        ChatChannel.Emotes,
        ChatChannel.Radio,
        ChatChannel.Notifications,
        ChatChannel.LOOC,
        ChatChannel.OOC,
        ChatChannel.Dead,
        ChatChannel.Admin,
        ChatChannel.AdminAlert,
        ChatChannel.AdminChat,
        ChatChannel.Server
    };

    private readonly Dictionary<ChatChannel, ChannelFilterCheckbox> _filterStates = new();

    public event Action<ChatChannel, bool>? OnChannelFilter;
    public event Action<string>? OnNewHighlights;

    public ChannelFilterPopup()
    {
        RobustXamlLoader.Load(this);

        HighlightButton.OnPressed += HighlightsEntered;

        HighlightEdit.Placeholder = new Rope.Leaf(Loc.GetString("hud-chatbox-highlights-placeholder"));

        // Load highlights if any were saved.
        var cfg = IoCManager.Resolve<IConfigurationManager>();
        string highlights = cfg.GetCVar(CCVars.ChatHighlights);

        if (!string.IsNullOrEmpty(highlights))
        {
            SetHighlights(highlights);
        }
    }

    public bool IsActive(ChatChannel channel)
    {
        return _filterStates.TryGetValue(channel, out var checkbox) && checkbox.Pressed;
    }

    public ChatChannel GetActive()
    {
        ChatChannel active = 0;

        foreach (var (key, value) in _filterStates)
        {
            if (value.IsHidden || !value.Pressed)
            {
                continue;
            }

            active |= key;
        }

        return active;
    }

    public void SetChannels(ChatChannel channels)
    {
        foreach (var channel in ChannelFilterOrder)
        {
            if (!_filterStates.TryGetValue(channel, out var checkbox))
            {
                checkbox = new ChannelFilterCheckbox(channel);
                _filterStates.Add(channel, checkbox);
                checkbox.OnPressed += CheckboxPressed;
                checkbox.Pressed = true;
            }

            if ((channels & channel) == 0)
            {
                if (checkbox.Parent == FilterVBox)
                {
                    FilterVBox.RemoveChild(checkbox);
                }
            }
            else if (checkbox.IsHidden)
            {
                FilterVBox.AddChild(checkbox);
            }
        }
    }

    public void SetHighlights(string highlights)
    {
        HighlightEdit.TextRope = new Rope.Leaf(highlights);
    }

    private void CheckboxPressed(ButtonEventArgs args)
    {
        var checkbox = (ChannelFilterCheckbox) args.Button;
        OnChannelFilter?.Invoke(checkbox.Channel, checkbox.Pressed);
    }

    private void HighlightsEntered(ButtonEventArgs _args)
    {
        OnNewHighlights?.Invoke(Rope.Collapse(HighlightEdit.TextRope));
    }

    public void UpdateUnread(ChatChannel channel, int? unread)
    {
        if (_filterStates.TryGetValue(channel, out var checkbox))
            checkbox.UpdateUnreadCount(unread);
    }
}
