import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { Rating } from 'src/app/domain/enums/rating';
import { IDataWithImageResponse } from 'src/app/domain/responses/image-response';
import { IOrder } from 'src/app/domain/responses/order';
import { IUserData } from 'src/app/domain/responses/user-data';
import { enumSelector } from 'src/app/shared/helpers/enum-selector';
import { FormValidationService } from 'src/app/shared/services/error-message.service';
import { DataService } from '../data.service';
import { OrderService } from '../order.service';
import { ReviewService } from '../review.service';

@Component({
  selector: 'app-review-order',
  templateUrl: './review-order.component.html',
  styleUrls: ['./review-order.component.css']
})
export class ReviewOrderComponent implements OnInit, OnDestroy {

  order: IOrder
  performers: any[] = []
  selectedPerformerId: number
  dataset: any[] = []
  loadingDataSet = false
  rateForm: FormGroup
  rates = enumSelector(Rating)
  noOneToReview = false

  private destroy$ = new Subject()

  constructor(private route: ActivatedRoute,
              private router: Router,
              private orderService: OrderService,
              private dataService: DataService,
              private reviewService: ReviewService,
              private dom: DomSanitizer,
              private fb: FormBuilder,
              public formValidationService: FormValidationService) { }

  ngOnInit(): void {
    this.order = this.route.snapshot.data.order.payload

    if (!this.order.isCompleted) {
      this.router.navigate(FrontUrl.orderForCustomerPage())
    }

    this.createForm()
    this.getPerformers()
  }

  createForm() {
    this.rateForm = this.fb.group({
      rate: [null, [Validators.required]]
    })
  }

  getPerformers() {
    this.orderService.getPerformers(this.order.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.performers = res.payload

          if (this.performers.length === 0) {
            this.noOneToReview = true
            return;
          }

          this.selectPerformer(this.performers[0].id)
        },
        err => {
          console.log(err)
        }
      )
  }

  selectPerformer(id: number) {
    if (id === this.selectedPerformerId) return
    this.selectedPerformerId = id
    this.loadPerformerJobs()
  }

  loadPerformerJobs() {
    this.loadingDataSet = true
    this.dataService.getDataSetForReview(this.order.id, this.selectedPerformerId)
      .pipe(takeUntil(this.destroy$), finalize(() => this.loadingDataSet = false))
      .subscribe(
        res => {
          this.dataset = res.payload.map(x => {
            x.image = this.dom.bypassSecurityTrustResourceUrl(`data:${x.contentType};base64,${x.image}`)
            return x
          })
          console.log(this.dataset);
        },
        err => {
          console.log(err)
        }
      )
  }

  onSubmit() {
    if (this.rateForm.invalid || this.selectedPerformerId == null) {
      this.rateForm.markAllAsTouched()
      return
    }

    this.reviewService.addReview({
      performerId: this.selectedPerformerId,
      orderId: this.order.id,
      rating: this.rateForm.get('rate').value
    })
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        () => {
          this.setReviewed(this.performers.find(x => x.id === this.selectedPerformerId))
        },
        err => {
          console.error(err)
        })
  }

  get isReviewed(): boolean {
    if (this.selectedPerformerId == null) return false

    return !!this.performers.find(x => x.id === this.selectedPerformerId).isReviewed
  }

  setReviewed(performer: any) {
    console.log('ok');
    
    performer.isReviewed = true
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
