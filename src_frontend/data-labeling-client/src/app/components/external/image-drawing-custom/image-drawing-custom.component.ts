import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { fabric } from 'fabric';
import { Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { FormValidationService } from 'src/app/shared/services/error-message.service';

@Component({
  selector: 'app-image-drawing-custom',
  templateUrl: './image-drawing-custom.component.html',
  styleUrls: ['./image-drawing-custom.component.scss']
})
export class ImageDrawingCustomComponent implements OnInit {

  @Input() public variants: string[];
  @Input() public src?: string;
  @Input() public width?: number;
  @Input() public height?: number;

  @Input() public forceSizeCanvas = true;
  @Input() public forceSizeExport = false;
  @Input() public enableRemoveImage = false;
  @Input() public enableLoadAnotherImage = false;
  @Input() public enableTooltip = true;
  @Input() public showCancelButton = true;

  // @ts-ignore
  @Input() public locale: string = 'en';
  /* @deprecated Use i18n.saveBtn */
  @Input() public saveBtnText = 'Save';
  /* @deprecated Use i18n.cancelBtn */
  @Input() public cancelBtnText = 'Cancel';
  /* @deprecated Use i18n.loading */
  @Input() public loadingText = 'Loading…';
  /* @deprecated Use i18n.loadError */
  @Input() public errorText = 'Error loading %@';

  @Input() public loadingTemplate?: TemplateRef<any>;
  @Input() public errorTemplate?: TemplateRef<any>;

  @Input() public outputMimeType = 'image/jpeg';
  @Input() public outputQuality = 0.8;

  @Input() public borderCss: string = 'none';

  @Input() public drawingSizes: { [name: string]: number } = {
    small: 5,
    medium: 10,
    large: 25,
  };

  @Input() public colors: { [name: string]: string } = {
    black: '#000',
    white: '#fff',
    yellow: '#ffeb3b',
    red: '#f44336',
    blue: '#2196f3',
    green: '#4caf50',
    purple: '#7a08af',
  };

  @Input() public refresh: Observable<void>
  @Input() public loadingBtn: Observable<boolean>
  labelProcessing = false

  @Output() public save: EventEmitter<{ image: Blob, variant: string }> = new EventEmitter<{ image: Blob, variant: string }>();
  @Output() public cancel: EventEmitter<void> = new EventEmitter<void>();

  public currentTool = 'brush';
  public currentSize = 'small';
  public currentColor = 'white';

  public canUndo = false;
  public canRedo = false;

  public isLoading = false;
  public hasError = false;
  public errorMessage = '';

  public dataLabelForm: FormGroup

  private canvas: fabric.Canvas;
  private stack: fabric.Object[] = [];

  public colorsName: string[] = [];
  public drawingSizesName: string[] = [];

  private imageUsed?: fabric.Image;

  started = false
  x = 0
  y = 0

  constructor(private fb: FormBuilder,
    public formValidationService: FormValidationService) { }

  public ngOnInit(): void {
    this.createForm()
    if (this.refresh) {
      this.refresh.subscribe(() => {
        this.dataLabelForm.reset()
        this.clearCanvas()
        if (this.src) {
          this.importPhotoFromSrc(this.src)
        }
      })
    }

    if (this.loadingBtn) {
      this.loadingBtn.subscribe(
        val => {
          this.labelProcessing = val
        }
      )
    }

    this.colorsName = Object.keys(this.colors);
    this.drawingSizesName = Object.keys(this.drawingSizes);

    this.canvas = new fabric.Canvas('canvas', {
      hoverCursor: 'pointer'
      // isDrawingMode: false,
    });
    this.canvas.backgroundColor = 'white';

    if (this.src) {
      this.importPhotoFromSrc(this.src);
    } else {
      if (!this.width || !this.height) {
        throw new Error('No width or hight given !');
      }

      this.canvas.setWidth(this.width);
      this.canvas.setHeight(this.height);
    }

    this.canvas.on('path:created', () => {
      this.stack = [];
      this.setUndoRedo();
    });

    this.selectTool(this.currentTool);
    this.selectColor(this.currentColor);
    this.selectDrawingSize(this.currentSize);

    const mouseDown = this.mouseDown
    const mouseUp = this.mouseup;
    const mouseMove = this.mousemove
    const instance = this;
    this.canvas.on('mouse:down', function (e) { mouseDown(e, instance) })
    this.canvas.on('mouse:move', function (e) { mouseMove(e, instance) })
    this.canvas.on('mouse:up', function (e) { mouseUp(e, instance) })
  }

  mouseDown(e, inst) {
    console.log('down');

    const mouse = inst.canvas.getPointer(e.e);
    inst.started = true;
    inst.x = mouse.x;
    inst.y = mouse.y;

    const square = new fabric.Rect({
      width: 0,
      height: 0,
      left: inst.x,
      top: inst.y,
      hasBorder: true,
      stroke: inst.currentColor,
      strokeWidth: inst.drawingSizes[inst.currentSize],
      fill: 'transparent'
    });

    inst.canvas.add(square);
    inst.canvas.renderAll();
    inst.canvas.setActiveObject(square);
  }

  mousemove(e, inst) {
    if (!inst.started) {
      return false;
    }

    const mouse = inst.canvas.getPointer(e.e);

    const w = Math.abs(mouse.x - inst.x);
    const h = Math.abs(mouse.y - inst.y);

    if (!w || !h) {
      return false;
    }

    const square = inst.canvas.getActiveObject();
    square.set('width', w).set('height', h);
    inst.canvas.renderAll();
  }

  /* Mouseup */
  mouseup(e, inst) {
    if (inst.started) {
      inst.started = false;
    }

    const square = inst.canvas.getActiveObject();

    // inst.canvas.add(square); 
    inst.canvas.renderAll();
    inst.setUndoRedo()
  }

  createForm() {
    this.dataLabelForm = this.fb.group({
      variant: [null, [Validators.required]]
    })
  }

  // Tools
  public selectTool(tool: string) {
    this.currentTool = tool;
  }

  public selectDrawingSize(size: string) {
    this.currentSize = size;
    if (this.canvas) {
      this.canvas.freeDrawingBrush.width = this.drawingSizes[size];
    }
  }

  public selectColor(color: string) {
    this.currentColor = color;
    if (this.canvas) {
      this.canvas.freeDrawingBrush.color = this.colors[color];
    }
  }

  // Actions

  public undo() {
    if (this.canUndo) {
      const lastId = this.canvas.getObjects().length - 1;
      const lastObj = this.canvas.getObjects()[lastId];
      this.stack.push(lastObj);
      this.canvas.remove(lastObj);
      this.setUndoRedo();
    }
  }

  public redo() {
    if (this.canRedo) {
      const firstInStack = this.stack.splice(-1, 1)[0];
      if (firstInStack) {
        this.canvas.insertAt(firstInStack, this.canvas.getObjects().length - 1, false);
      }
      this.setUndoRedo();
    }
  }

  public clearCanvas() {
    if (this.canvas) {
      this.canvas.remove(...this.canvas.getObjects());
      this.setUndoRedo();
    }
  }

  public saveImage() {
    if (this.dataLabelForm.invalid) {
      this.dataLabelForm.markAllAsTouched()
      return
    }
    const variant = this.dataLabelForm.get('variant').value

    if (!this.forceSizeExport || (this.forceSizeExport && this.width && this.height)) {
      const canvasScaledElement: HTMLCanvasElement = document.createElement('canvas');
      const canvasScaled = new fabric.Canvas(canvasScaledElement);
      canvasScaled.backgroundColor = 'white';

      new Observable<fabric.Canvas>(observer => {
        if (this.imageUsed) {
          if (this.forceSizeExport) {
            canvasScaled.setWidth(this.width);
            canvasScaled.setHeight(this.height);

            this.imageUsed.cloneAsImage(imageCloned => {
              imageCloned.scaleToWidth(this.width, false);
              imageCloned.scaleToHeight(this.height, false);

              canvasScaled.setBackgroundImage(imageCloned, (img: HTMLImageElement) => {
                if (!img) {
                  observer.error(new Error('Impossible to draw the image on the temporary canvas'));
                }

                observer.next(canvasScaled);
                observer.complete();
              }, {
                crossOrigin: 'anonymous',
                originX: 'left',
                originY: 'top'
              });
            });
          } else {
            canvasScaled.setBackgroundImage(this.imageUsed, (img: HTMLImageElement) => {
              if (!img) {
                observer.error(new Error('Impossible to draw the image on the temporary canvas'));
              }

              canvasScaled.setWidth(img.width);
              canvasScaled.setHeight(img.height);

              observer.next(canvasScaled);
              observer.complete();
            }, {
              crossOrigin: 'anonymous',
              originX: 'left',
              originY: 'top'
            });
          }
        } else {
          canvasScaled.setWidth(this.width);
          canvasScaled.setHeight(this.height);
        }
      }).pipe(
        switchMap(() => {
          let process = of(canvasScaled);

          if (this.canvas.getObjects().length > 0) {
            const ratioX = canvasScaled.getWidth() / this.canvas.getWidth();
            const ratioY = canvasScaled.getHeight() / this.canvas.getHeight();

            this.canvas.getObjects().forEach((originalObject: fabric.Object, i: number) => {
              process = process.pipe(switchMap(() => {
                return new Observable<fabric.Canvas>(observerObject => {
                  originalObject.clone((clonedObject: fabric.Object) => {
                    clonedObject.set('left', originalObject.left * ratioX);
                    clonedObject.set('top', originalObject.top * ratioY);
                    clonedObject.scaleToWidth(originalObject.width * ratioX);
                    clonedObject.scaleToHeight(originalObject.height * ratioY);

                    canvasScaled.insertAt(clonedObject, i, false);
                    canvasScaled.renderAll();

                    observerObject.next(canvasScaled);
                    observerObject.complete();
                  });
                });
              }));
            });
          }
          return process;
        }),
      ).subscribe(() => {
        canvasScaled.renderAll();
        canvasScaled.getElement().toBlob(
          (data: Blob) => {
            this.save.emit({ image: data, variant: variant });
          },
          this.outputMimeType,
          this.outputQuality
        );
      });
    } else {
      this.canvas.getElement().toBlob(
        (data: Blob) => {
          this.save.emit({ image: data, variant: variant });
        },
        this.outputMimeType,
        this.outputQuality
      );
    }
  }

  public cancelAction() {
    this.cancel.emit();
  }

  private setUndoRedo() {
    this.canUndo = this.canvas.getObjects().length > 0;
    this.canRedo = this.stack.length > 0;
    // this.canvas.renderAll();
  }

  public importPhotoFromFile(event: Event | any) {
    if (event.target.files && event.target.files.length > 0) {
      const file = event.target.files[0];
      if (file.type.match('image.*')) {
        this.importPhotoFromBlob(file);
      } else {
        throw new Error('Not an image !');
      }
    }
  }

  public removeImage() {
    if (this.imageUsed) {
      this.imageUsed.dispose();
      this.imageUsed = null;
    }
    this.canvas.backgroundImage = null;

    if (this.width && this.height) {
      this.canvas.setWidth(this.width);
      this.canvas.setHeight(this.height);
    }

    this.canvas.renderAll();
  }

  public get hasImage(): boolean {
    return !!this.canvas.backgroundImage;
  }

  private importPhotoFromSrc(src: string) {
    this.isLoading = true;
    let isFirstTry = true;
    const imgEl = new Image();
    imgEl.setAttribute('crossOrigin', 'anonymous');
    imgEl.src = src;
    imgEl.onerror = () => {
      // Retry with cors proxy
      if (isFirstTry) {
        imgEl.src = 'https://cors-anywhere.herokuapp.com/' + this.src;
        isFirstTry = false;
      } else {
        this.isLoading = false;
        this.hasError = true;
        this.errorMessage = 'Something went wrong';
      }
    };
    imgEl.onload = () => {
      this.isLoading = false;
      this.imageUsed = new fabric.Image(imgEl);

      this.imageUsed.cloneAsImage(image => {
        let width = imgEl.width;
        let height = imgEl.height;

        if (this.width) {
          width = this.width;
        }
        if (this.height) {
          height = this.height;
        }

        image.scaleToWidth(width, false);
        image.scaleToHeight(height, false);

        this.canvas.setBackgroundImage(image, ((img: HTMLImageElement) => {
          if (img) {
            if (this.forceSizeCanvas) {
              this.canvas.setWidth(width);
              this.canvas.setHeight(height);
            } else {
              this.canvas.setWidth(image.getScaledWidth());
              this.canvas.setHeight(image.getScaledHeight());
            }
          }
        }), {
          crossOrigin: 'anonymous',
          originX: 'left',
          originY: 'top'
        });
      });
    };
  }

  private importPhotoFromBlob(file: Blob | File) {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = (evtReader: any) => {
      if (evtReader.target.readyState == FileReader.DONE) {
        this.importPhotoFromSrc(evtReader.target.result);
      }
    };
  }

  public importPhotoFromUrl() {
    const url = prompt('input image url');
    if (url) {
      this.importPhotoFromSrc(url);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.src && !changes.src.firstChange && changes.src.currentValue) {
      if (typeof changes.src.currentValue === 'string') {
        this.importPhotoFromSrc(changes.src.currentValue);
      } else if (changes.src.currentValue instanceof Blob) {
        this.importPhotoFromBlob(changes.src.currentValue);
      }
    }
  }
}

