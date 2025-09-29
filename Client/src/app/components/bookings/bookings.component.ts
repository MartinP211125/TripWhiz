import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Booking, BookingService, BookingStatus } from '../../services/booking.service';
import { UserService } from '../../services/user.service';
import { OfferService, Offer } from '../../services/offer.service';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.css']
})
export class BookingsComponent implements OnInit {
  private bookingService = inject(BookingService);
  private userService = inject(UserService);
  private router = inject(Router);
  private offerService = inject(OfferService);
  bookings: Booking[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  isCancelling = false;
  cancellingBookingId: string | null = null;
  offerMap = new Map<string, Offer>();

  // Expose enum to template
  BookingStatus = BookingStatus;
  ngOnInit(): void {
    // Check if user is logged in
    if (!this.userService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookingService.getUserBookings().subscribe({
      next: (bookings) => {
        this.bookings = bookings;
        this.loadOffers();  // ✅ Moved here, after bookings are set
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load bookings. Please try again.';
        this.isLoading = false;
        console.error('Error loading bookings:', error);
      }
    });
  }

  onCancelBooking(booking: Booking): void {
    if (this.isCancelling) return;
    let offer = this.offerMap.get(booking.offerId);
    const confirmed = confirm(`Are you sure you want to cancel the booking for "${offer?.title}"?`);
    if (!confirmed) return;

    this.isCancelling = true;
    this.cancellingBookingId = booking.id;
    this.errorMessage = '';
    this.successMessage = '';

    this.bookingService.cancelBooking(booking.id).subscribe({
      next: () => {
        this.successMessage = `Booking for "${offer?.title}" has been cancelled successfully.`;
        this.loadBookings(); // Reload to get updated status
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to cancel booking. Please try again.';
        this.isCancelling = false;
        this.cancellingBookingId = null;
      },
      complete: () => {
        this.isCancelling = false;
        this.cancellingBookingId = null;
      }
    });
  }

  onViewOffer(booking: Booking): void {
    this.router.navigate(['/offer', booking.offerId]);
  }

  onBackToOffers(): void {
    this.router.navigate(['/main']);
  }

formatDate(dateString?: string): string {
  if (!dateString) return '';
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  });
}

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  }

  getStatusText(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Confirmed:
        return 'Confirmed';
      case BookingStatus.Cancelled:
        return 'Cancelled';
      case BookingStatus.Pending:
        return 'Pending';
      default:
        return 'Unknown';
    }
  }

  getStatusClass(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Confirmed:
        return 'status-confirmed';
      case BookingStatus.Cancelled:
        return 'status-cancelled';
      case BookingStatus.Pending:
        return 'status-pending';
      default:
        return '';
    }
  }

  getStatusIcon(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Confirmed:
        return 'fas fa-check-circle';
      case BookingStatus.Cancelled:
        return 'fas fa-times-circle';
      case BookingStatus.Pending:
        return 'fas fa-clock';
      default:
        return 'fas fa-question-circle';
    }
  }

  hasBookings(): boolean {
    return this.bookings.length > 0;
  }

  getActiveBookings(): Booking[] {
    return this.bookings.filter(booking => 
      booking.status === BookingStatus.Confirmed || 
      booking.status === BookingStatus.Pending
    );
  }

  getPastBookings(): Booking[] {
    return this.bookings.filter(booking => 
      booking.status === BookingStatus.Cancelled
    );
  }

  loadOffers(){
    this.bookings.forEach(booking => {
    this.offerService.getOfferById(booking.offerId).subscribe({
      next: (offer) => {
       this.offerMap.set(booking.offerId, offer);
      }
    });
  })
}
}