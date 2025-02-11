﻿using System;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic;

public partial class SecondLevelNavMenuItem : IDisposable
{
    [Parameter]
    public ApplicationMenuItem MenuItem { get; set; } = default!;

    [Parameter]
    public  Dropdown ParentDropdown { get; set; } = default!;

    private Dropdown _dropdown;

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        ParentDropdown?.Hide();
        _dropdown?.Hide();
    }

    public virtual void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
