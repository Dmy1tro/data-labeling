import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImageDrawingCustomComponent } from './image-drawing-custom.component';
import { SharedModule } from 'src/app/shared/shared.module';



@NgModule({
  declarations: [
    ImageDrawingCustomComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [
    ImageDrawingCustomComponent
  ]
})
export class ImageDrawingCustomModule { }
