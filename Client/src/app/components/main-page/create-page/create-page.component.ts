import { Component, OnInit, inject, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CreateOfferRequest, OfferService, OfferType } from '../../../services/offer.service';

@Component({
  selector: 'app-create-offer',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-page.component.html',
  styleUrls: ['./create-page.component.css']
})
export class CreateOfferComponent implements OnInit {
  @Output() offerCreated = new EventEmitter<void>();
  @Output() closeModal = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private offerService = inject(OfferService);

  createOfferForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  offerTypes = [
    { value: OfferType.Standard, label: 'Standard', description: 'Regular offer visible to all users' },
    { value: OfferType.Trending, label: 'Trending', description: 'Popular offer that gets highlighted' },
    { value: OfferType.Recommended, label: 'Recommended', description: 'Personalized offer for specific users' }
  ];

  constructor() {
    this.createOfferForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      imageUrl: ['', [Validators.required, Validators.pattern(/^https?:\/\/.+\.(jpg|jpeg|png|gif|webp)$/i)]],
      price: ['', [Validators.required, Validators.min(1), Validators.max(999999)]],
      fromDate: ['', Validators.required],
      toDate: [''],
      accomodationId: ['', Validators.required],
      offerType: [OfferType.Standard, Validators.required]
    });
  }

  ngOnInit(): void {
    // Set minimum date to today
    const today = new Date().toISOString().split('T')[0];
    this.createOfferForm.get('fromDate')?.setValue(today);
  }

  get title() { return this.createOfferForm.get('title'); }
  get description() { return this.createOfferForm.get('description'); }
  get imageUrl() { return this.createOfferForm.get('imageUrl'); }
  get price() { return this.createOfferForm.get('price'); }
  get fromDate() { return this.createOfferForm.get('fromDate'); }
  get toDate() { return this.createOfferForm.get('toDate'); }
  get accomodationId() { return this.createOfferForm.get('accomodationId'); }
  get offerType() { return this.createOfferForm.get('offerType'); }

  onSubmit(): void {
    if (this.createOfferForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      // Validate that toDate is after fromDate if provided
      const fromDate = new Date(this.createOfferForm.value.fromDate);
      const toDate = this.createOfferForm.value.toDate ? new Date(this.createOfferForm.value.toDate) : null;
      
      if (toDate && toDate <= fromDate) {
        this.errorMessage = 'End date must be after start date.';
        this.isLoading = false;
        return;
      }

      const createRequest: CreateOfferRequest = {
        title: this.createOfferForm.value.title,
        description: this.createOfferForm.value.description,
        imageUrl: this.createOfferForm.value.imageUrl,
        price: parseFloat(this.createOfferForm.value.price),
        fromDate: this.createOfferForm.value.fromDate,
        toDate: this.createOfferForm.value.toDate || undefined,
        accomodationId: this.createOfferForm.value.accomodationId,
        offerType: this.createOfferForm.value.offerType
      };

      this.offerService.createOffer(createRequest).subscribe({
        next: (response) => {
          this.successMessage = 'Offer created successfully!';
          setTimeout(() => {
            this.offerCreated.emit();
            this.onClose();
          }, 2000);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to create offer. Please try again.';
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.createOfferForm.controls).forEach(key => {
      const control = this.createOfferForm.get(key);
      control?.markAsTouched();
    });
  }

  onClose(): void {
    this.closeModal.emit();
  }

  generateAccommodationId(): void {
    // Generate a random GUID for demonstration
    const guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      const r = Math.random() * 16 | 0;
      const v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
    this.createOfferForm.get('accomodationId')?.setValue(guid);
  }

  getOfferTypeDescription(type: OfferType): string {
    const offerType = this.offerTypes.find(t => t.value === type);
    return offerType?.description || '';
  }
}