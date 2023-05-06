import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IOrder } from 'src/app/domain/responses/order';

@Component({
  selector: 'app-perform-data-info',
  templateUrl: './perform-data-info.component.html',
  styleUrls: ['./perform-data-info.component.css']
})
export class PerformDataInfoComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<PerformDataInfoComponent>,
             @Inject(MAT_DIALOG_DATA) public settings: IPerformDataInfoSettings) { }

  ngOnInit(): void {
  }

}

export interface IPerformDataInfoSettings {
  isLabel: boolean,
  order: IOrder 
}