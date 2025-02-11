import { AbstractNgModelComponent } from '@abp/ng.core';
import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  standalone: false,
  selector: 'abp-checkbox',
  template: `
    <div class="mb-3">
      <input
        type="checkbox"
        [(ngModel)]="value"
        [id]="checkboxId"
        [readonly]="checkboxReadonly"
        [ngClass]="checkboxClass"
        [ngStyle]="checkboxStyle"
        (blur)="checkboxBlur.next()"
        (focus)="checkboxFocus.next()"
      />
      @if (label) {
        <label [ngClass]="labelClass" [for]="checkboxId">
          {{ label | abpLocalization }}
        </label>
      }
    </div>
  `,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormCheckboxComponent),
      multi: true,
    },
  ],
})
export class FormCheckboxComponent extends AbstractNgModelComponent {
  @Input() label?: string;
  @Input() labelClass = 'form-check-label';
  @Input() checkboxId!: string;
  @Input() checkboxStyle:
    | {
        [klass: string]: any;
      }
    | null
    | undefined;
  @Input() checkboxClass = 'form-check-input';
  @Input() checkboxReadonly = false;
  @Output() checkboxBlur = new EventEmitter<void>();
  @Output() checkboxFocus = new EventEmitter<void>();
}
