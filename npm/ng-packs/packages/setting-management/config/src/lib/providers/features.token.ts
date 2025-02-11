import { ConfigStateService, featuresFactory } from '@abp/ng.core';
import { inject, InjectionToken, provideAppInitializer } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

export const SETTING_MANAGEMENT_FEATURES = new InjectionToken<Observable<{ enable: boolean }>>(
  'SETTING_MANAGEMENT_FEATURES',
  {
    providedIn: 'root',
    factory: () => {
      const configState = inject(ConfigStateService);
      const featureKey = 'SettingManagement.Enable';
      const mapFn = (features: Record<string, string>) => ({
        enable: features[featureKey].toLowerCase() !== 'false',
      });
      return featuresFactory(configState, [featureKey], mapFn);
    },
  },
);

export const SETTING_MANAGEMENT_ROUTE_VISIBILITY = new InjectionToken<Observable<boolean>>(
  'SETTING_MANAGEMENT_ROUTE_VISIBILITY',
  {
    providedIn: 'root',
    factory: () => {
      const stream = inject(SETTING_MANAGEMENT_FEATURES);
      return stream.pipe(map(features => features.enable));
    },
  },
);

export const SETTING_MANAGEMENT_FEATURES_PROVIDERS = [
  provideAppInitializer(() => {
    inject(SETTING_MANAGEMENT_ROUTE_VISIBILITY);
  }),
];
