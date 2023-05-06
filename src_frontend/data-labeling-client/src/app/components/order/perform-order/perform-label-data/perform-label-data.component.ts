import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { IDataWithImageResponse } from 'src/app/domain/responses/image-response';
import { IOrder } from 'src/app/domain/responses/order';
import { FormValidationService } from 'src/app/shared/services/error-message.service';
import { DataService } from '../../data.service';
import { OrderService } from '../../order.service';

@Component({
  selector: 'app-perform-label-data',
  templateUrl: './perform-label-data.component.html',
  styleUrls: ['./perform-label-data.component.css']
})
export class PerformLabelDataComponent implements OnInit, OnDestroy {

  @Input() order: IOrder
  @Output() orderChanged = new EventEmitter()
  imgData: IDataWithImageResponse
  loading = false
  displayDrawing = true
  imgUrl = ''
  refreshObs: Observable<void>
  labelProcessingObs: Observable<boolean>

  private refreshEmiter = new EventEmitter()
  private labelProcessingEmiter = new EventEmitter<boolean>()
  private destroy$ = new Subject()

  constructor(private dataService: DataService,
              private orderService: OrderService,
              private toastr: ToastrService,
              private router: Router,
              public formValidationService: FormValidationService) { }

  ngOnInit(): void {
    this.refreshObs = this.refreshEmiter.asObservable()
    this.labelProcessingObs = this.labelProcessingEmiter.asObservable()
    this.getDataForLabeling()
  }

  getDataForLabeling() {
    this.labelProcessingEmiter.next(true)
    this.loading = true
    this.displayDrawing = false
    this.imgUrl = ''
    this.dataService.getDataForLabeling(this.order.id)
      .pipe(takeUntil(this.destroy$), finalize(() => {
        this.loading = false
        this.labelProcessingEmiter.next(false)
      }))
      .subscribe(
        res => {
          if (res == null) {
            this.toastr.success('Order is completed. Choose another one!', 'Success')
            this.router.navigate(FrontUrl.ordersForPerformerPage())
          }

          this.imgData = res.payload
          this.imgUrl = `data:${this.imgData.contentType};base64,${this.imgData.image}`
          this.displayDrawing = true
          this.refreshDrawing()
        },
        err => {
          this.displayDrawing = true
          console.log(err);
        }
      )
  }

  save(data: {image: Blob, variant: string}) {
    this.labelProcessingEmiter.next(true)
    const formData = new FormData()
    formData.append('dataId', this.imgData.data.id.toString())
    formData.append('variant', data.variant)
    formData.append('imageFile', data.image, 'labeled_img.jpeg')

    this.dataService.labelData(formData)
      .pipe(takeUntil(this.destroy$), finalize(() => this.labelProcessingEmiter.next(false)))
      .subscribe(
        () => {
          this.toastr.success('Submited')
          this.orderChanged.emit()
          this.getDataForLabeling()
        },
        err => {
          this.toastr.error(err.error.detail ?? 'Something went wrong')
          console.log('error', err);
        })
  }

  refreshDrawing() {
    this.refreshEmiter.next()
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
