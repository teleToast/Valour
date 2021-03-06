﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

/*  Valour - A free and secure chat client
 *  Copyright (C) 2021 Vooper Media LLC
 *  This program is subject to the GNU Affero General Public license
 *  A copy of the license should be included - if not, see <http://www.gnu.org/licenses/>
 */

namespace Valour.Server.Oauth
{
    /// <summary>
    /// Permissions are basic flags used to denote if actions are allowed
    /// to be taken on one's behalf
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Permission node to have complete control
        /// </summary>
        public const ulong FULL_CONTROL = 0xFFFFFFFFFFFFFFFF;

        /// <summary>
        /// The name of this permission
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of this permission
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The value of this permission
        /// </summary>
        public ulong Value { get; set; }

        /// <summary>
        /// Initializes the permission
        /// </summary>
        public Permission(ulong value, string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Value = value; 
        }

        /// <summary>
        /// Returns whether the given code includes the given permission
        /// </summary>
        public static bool HasPermission(ulong code, Permission permission)
        {
            // Case if full control is granted
            if (code == FULL_CONTROL) return true;

            // Otherwise check for specific permission
            return (code & permission.Value) == permission.Value;
        }

        /// <summary>
        /// Creates and returns a permission code from given permissions 
        /// </summary>
        public static ulong CreateCode(params Permission[] permissions)
        {
            ulong code = 0x00;

            foreach (Permission permission in permissions)
            {
                if (permission != null)
                {
                    code |= permission.Value;
                }
            }

            return code;
        }
    }

    public class ChannelPermission : Permission
    {
        public ChannelPermission(ulong value, string name, string description) : base(value, name, description)
        {
        }
    }

    public class UserPermission : Permission
    {
        public UserPermission(ulong value, string name, string description) : base(value, name, description)
        {
        }
    }

    public class PlanetPermission : Permission
    {
        public PlanetPermission(ulong value, string name, string description) : base(value, name, description)
        {
        }
    }

    /// <summary>
    /// This class contains all user permissions and helper methods for working
    /// with them.
    /// </summary>
    public class UserPermissions
    {
        /// <summary>
        /// Contains every user permission
        /// </summary>
        public static readonly UserPermission[] Permissions = new UserPermission[]
        {
            FullControl,
            Minimum,
            View,
            Membership,
            Invites
        };

        // Use shared full control definition
        public static readonly UserPermission FullControl = new UserPermission(0xFFFFFFFFFFFFFFFF, "Full Control", "Control every part of your account.");

        // Every subsequent permission has double the value (the next bit)
        // An update should NEVER change the order or value of old permissions
        // As that would be a massive security issue
        public static readonly UserPermission Minimum = new UserPermission(0x01, "Minimum", "Allows this app to only view your account ID when authorized.");
        public static readonly UserPermission View = new UserPermission(0x02, "View", "Allows this app to access basic information about your account.");
        public static readonly UserPermission Membership = new UserPermission(0x04, "Membership", "Allows this app to view the planets you are a member of.");
        public static readonly UserPermission Invites = new UserPermission(0x08, "Invites", "Allows this app to view the planets you are invited to.");
    }

    /// <summary>
    /// This class contains all channel permissions and helper methods for working
    /// with them
    /// </summary>
    public class ChannelPermissions
    {

        public static readonly ulong Default =
            Permission.CreateCode(View, ViewMessages, PostMessages);

        /// <summary>
        /// Contains every channel permission
        /// </summary>
        public static readonly ChannelPermission[] Permissions = new ChannelPermission[]
        {
            FullControl,
            View,
            ViewMessages,
            PostMessages,
            ManageChannel,
            ManagePermissions,
            Embed,
            AttachContent
        };

        // Use shared full control definition
        public static readonly ChannelPermission FullControl = new ChannelPermission(0xFFFFFFFFFFFFFFFF, "Full Control", "Allow members full control of the channel");

        public static readonly ChannelPermission View = new ChannelPermission(0x01, "View", "Allow members to view this channel in the channel list.");
        public static readonly ChannelPermission ViewMessages = new ChannelPermission(0x02, "View Messages", "Allow members to view the messages within this channel.");
        public static readonly ChannelPermission PostMessages = new ChannelPermission(0x04, "Post", "Allow members to post messages to this channel.");
        public static readonly ChannelPermission ManageChannel = new ChannelPermission(0x08, "Manage", "Allow members to manage this channel's details.");
        public static readonly ChannelPermission ManagePermissions = new ChannelPermission(0x10, "Permissions", "Allow members to manage permissions for this channel.");
        public static readonly ChannelPermission Embed = new ChannelPermission(0x20, "Embed", "Allow members to post embedded content to this channel.");
        public static readonly ChannelPermission AttachContent = new ChannelPermission(0x40, "Attach Content", "Allow members to upload files to this channel.");
    }

    /// <summary>
    /// This class contains all planet permissions and helper methods for working
    /// with them
    /// </summary>
    public class PlanetPermissions
    {
        public static readonly ulong Default = 
            Permission.CreateCode(View);

        /// <summary>
        /// Contains every planet permission
        /// </summary>
        public static readonly PlanetPermission[] Permissions = new PlanetPermission[]
        {
            FullControl,
            View,
            Invite,
            DisplayRole
        };

        // Use shared full control definition
        public static readonly PlanetPermission FullControl = new PlanetPermission(0xFFFFFFFFFFFFFFFF, "Full Control", "Allow members full control of the planet (owner)");

        public static readonly PlanetPermission View = new PlanetPermission(0x01, "View", "Allow members to view the planet. This is implicitly granted to members."); // Implicitly granted to members
        public static readonly PlanetPermission Invite = new PlanetPermission(0x02, "Invite", "Allow members to send invites to the planet.");
        public static readonly PlanetPermission DisplayRole = new PlanetPermission(0x04, "Display Role", "Enables a role to be displayed seperately in the role list.");
        public static readonly PlanetPermission Manage = new PlanetPermission(0x08, "Manage Planet", "Allow members to modify base planet settings.");
        public static readonly PlanetPermission Kick = new PlanetPermission(0x10, "Kick Members", "Allow members to kick other members.");
        public static readonly PlanetPermission Ban = new PlanetPermission(0x20, "Ban Members", "Allow members to ban other members.");
        public static readonly PlanetPermission ManageCategories = new PlanetPermission(0x40, "Manage Categories", "Allow members to manage categories.");
        public static readonly PlanetPermission ManageChannels = new PlanetPermission(0x80, "Manage Categories", "Allow members to manage channels.");
    }

    public enum PermissionState
    {
        Undefined, True, False
    }

    /// <summary>
    /// Permission codes use two ulongs to represent
    /// three possible states for every permission
    /// </summary>
    public struct PermissionNodeCode
    {
        // Just for reference,
        // If the mask bit is 0, then it is always undefined
        // If the mask but is 1, then if the code bit is 1 it is true. Otherwise it is false.
        // This basically compresses 64 booleans (64 bytes) into 2 ulongs (16 bytes)

        public ulong Code { get; set; }
        public ulong Mask { get; set; }

        public PermissionNodeCode(ulong code, ulong mask)
        {
            this.Code = code;
            this.Mask = mask;
        }

        public PermissionState GetState(Permission permission)
        {
            if ((Mask & permission.Value) == 0x00)
            {
                return PermissionState.Undefined;
            }

            if ((Code & permission.Value) == 0x00)
            {
                return PermissionState.False;
            }

            return PermissionState.True;
        }
    }
}
