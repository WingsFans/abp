﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Public.Menus;

namespace Volo.CmsKit.Public.Web.Menus;

public class CmsKitPublicMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == CmsKitMenus.Public)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
        if (GlobalFeatureManager.Instance.IsEnabled<MenuFeature>() && await featureChecker.IsEnabledAsync(CmsKitFeatures.MenuEnable))
        {
            var menuAppService = context.ServiceProvider.GetRequiredService<IMenuItemPublicAppService>();

            var menuItems = await menuAppService.GetListAsync();

            if (!menuItems.IsNullOrEmpty())
            {
                foreach (var menuItemDto in menuItems.Where(x => x.ParentId == null && x.IsActive))
                {
                    AddChildItems(menuItemDto, menuItems, context.Menu);
                }
            }
        }
    }

    private void AddChildItems(MenuItemDto menuItem, List<MenuItemDto> source, IHasMenuItems parent = null)
    {
        var applicationMenuItem = CreateApplicationMenuItem(menuItem);

        foreach (var item in source.Where(x => x.ParentId == menuItem.Id && x.IsActive))
        {
            AddChildItems(item, source, applicationMenuItem);
        }

        parent?.Items.Add(applicationMenuItem);
    }

    private ApplicationMenuItem CreateApplicationMenuItem(MenuItemDto menuItem)
    {
        var menu = new ApplicationMenuItem(
            menuItem.DisplayName,
            menuItem.DisplayName,
            menuItem.Url,
            menuItem.Icon,
            menuItem.Order,
            menuItem.Target,
            menuItem.ElementId,
            menuItem.CssClass
        );
        if (!menuItem.RequiredPermissionName.IsNullOrWhiteSpace())
        {
            menu.RequirePermissions(menuItem.RequiredPermissionName);
        }

        return menu;
    }
}
