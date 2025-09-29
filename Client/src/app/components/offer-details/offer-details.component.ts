import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Offer, OfferService } from '../../services/offer.service';
import { BookingDto, BookingService, TransportationType } from '../../services/booking.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-offer-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './offer-details.component.html',
  styleUrls: ['./offer-details.component.css']
})
export class OfferDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private offerService = inject(OfferService);
  private bookingService = inject(BookingService);
  private userService = inject(UserService);
  private fb = inject(FormBuilder);

  offer: Offer | null = null;
  isLoading = true;
  isBooking = false;
  errorMessage = '';
  successMessage = '';
  showBookingForm = false;
  transportationEnum = TransportationType;
  transportationTypes = Object.keys(this.transportationEnum)
    .filter(key => isNaN(Number(key))) as (keyof typeof TransportationType)[];

  bookingForm: FormGroup;

  constructor() {
    this.bookingForm = this.fb.group({
      numberOfPeople: [1, [Validators.required, Validators.min(1), Validators.max(20)]],
      transportationType: ['', Validators.required] // <-- FIXED
    });
  }

  ngOnInit(): void {
    if (!this.userService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    const offerId = this.route.snapshot.paramMap.get('id');
    if (offerId) {
      this.loadOffer(offerId);
    } else {
      this.router.navigate(['/main']);
    }
  }

  loadOffer(offerId: string): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.offerService.getOfferById(offerId).subscribe({
      next: (offer) => {
        this.offer = offer;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load offer details. Please try again.';
        this.isLoading = false;
        console.error('Error loading offer:', error);
      }
    });
  }

  onBookNow(): void {
    if (!this.offer) return;
    this.showBookingForm = true;
  }

  onSubmitBooking(): void {
    if (!this.offer || this.bookingForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    this.isBooking = true;
    this.errorMessage = '';
    this.successMessage = '';

    const bookingData: BookingDto = {
      offerId: this.offer.id,
      numberOfPeople: this.bookingForm.value.numberOfPeople,
      transportationType: +this.bookingForm.value.transportationType
    };

    this.bookingService.createBooking(bookingData).subscribe({
      next: (booking) => {
        this.successMessage = 'Booking successful! You can view your booking in "My Bookings".';
        this.showBookingForm = false;
        this.bookingForm.reset();
        this.bookingForm.get('numberOfPeople')?.setValue(1);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to create booking. Please try again.';
        this.isBooking = false;
      },
      complete: () => {
        this.isBooking = false;
      }
    });
  }

  onCancelBooking(): void {
    this.showBookingForm = false;
    this.bookingForm.reset();
    this.bookingForm.get('numberOfPeople')?.setValue(1);
    this.errorMessage = '';
  }

  private markFormGroupTouched(): void {
    Object.keys(this.bookingForm.controls).forEach(key => {
      const control = this.bookingForm.get(key);
      control?.markAsTouched();
    });
  }

  onBackToOffers(): void {
    this.router.navigate(['/main']);
  }

  onViewBookings(): void {
    this.router.navigate(['/bookings']);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'long', 
      day: 'numeric' 
    });
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  }

  calculateTotalPrice(): number {
    if (!this.offer) return 0;
    return this.offer.price * (this.bookingForm.get('numberOfPeople')?.value || 1);
  }

  get numberOfPeople() { return this.bookingForm.get('numberOfPeople'); }
  get transportationType() { return this.bookingForm.get('transportationType'); }
}
