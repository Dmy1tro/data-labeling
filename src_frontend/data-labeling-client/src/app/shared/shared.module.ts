import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material/material.module';
import { ToastrModule } from 'ngx-toastr';
import { SideMenuComponent } from './layouts/side-menu/side-menu.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FormValidationService } from './services/error-message.service';
import { ConfirmModalComponent } from './components/confirm-modal/confirm-modal.component';
import { LoaderComponent } from './components/loader/loader.component';
import { PerformDataInfoComponent } from './components/perform-data-info/perform-data-info.component';


@NgModule({
  declarations: [
    SideMenuComponent,
    ConfirmModalComponent,
    LoaderComponent,
    PerformDataInfoComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot()
  ],
  exports: [
    MaterialModule,
    SideMenuComponent,
    FormsModule,
    ReactiveFormsModule,
    ConfirmModalComponent,
    LoaderComponent,
    PerformDataInfoComponent
  ]
})
export class SharedModule { 
  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule,
      providers: [
        FormValidationService
      ]
    }
  }
}
