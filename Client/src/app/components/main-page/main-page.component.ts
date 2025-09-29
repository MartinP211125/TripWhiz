import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CategorizedOffers, Offer, OfferService } from '../../services/offer.service';
import { UserService } from '../../services/user.service';

type CategoryKey = 'recommended' | 'trending' | 'standard';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  private offerService = inject(OfferService);
  private userService = inject(UserService);
  private router = inject(Router);

  categorizedOffers: CategorizedOffers = {
    recommended: [],
    trending: [],
    standard: []
  };

  filteredOffers: CategorizedOffers = {
    recommended: [],
    trending: [],
    standard: []
  };

  categoryList: CategoryKey[] = ['recommended', 'trending', 'standard'];

  // Pagination
  pageIndex: Record<CategoryKey, number> = {
    recommended: 0,
    trending: 0,
    standard: 0
  };
  cardsPerPage = 4;

  isLoading = true;
  errorMessage = '';
  showCreateOfferModal = false;
  searchQuery = '';

  ngOnInit(): void {
    if (!this.userService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadOffers();
  }

  loadOffers(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.offerService.getCategorizedOffers().subscribe({
      next: (offers) => {
        this.categorizedOffers = offers;
        this.filteredOffers = {
          recommended: [...offers.recommended],
          trending: [...offers.trending],
          standard: [...offers.standard]
        };
        this.resetPagination();
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load offers. Please try again.';
        this.isLoading = false;
        console.error('Error loading offers:', error);
      }
    });
  }

  onSearchChange(): void {
    const query = this.searchQuery.toLowerCase().trim();

    if (!query) {
      this.filteredOffers = {
        recommended: [...this.categorizedOffers.recommended],
        trending: [...this.categorizedOffers.trending],
        standard: [...this.categorizedOffers.standard]
      };
    } else {
      const filter = (offers: Offer[]) =>
        offers.filter(offer =>
          offer.title.toLowerCase().includes(query) ||
          offer.description.toLowerCase().includes(query)
        );

      this.filteredOffers = {
        recommended: filter(this.categorizedOffers.recommended),
        trending: filter(this.categorizedOffers.trending),
        standard: filter(this.categorizedOffers.standard)
      };
    }

    this.resetPagination();
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.onSearchChange();
  }

  hasSearchResults(): boolean {
    return this.searchQuery.trim().length > 0;
  }

  hasOffers(): boolean {
    return (
      this.filteredOffers.recommended.length +
      this.filteredOffers.trending.length +
      this.filteredOffers.standard.length
    ) > 0;
  }

  resetPagination(): void {
    this.pageIndex = {
      recommended: 0,
      trending: 0,
      standard: 0
    };
  }

  // Get visible offers for a category (paged)
  getVisibleOffers(category: CategoryKey): Offer[] {
    const start = this.pageIndex[category] * this.cardsPerPage;
    return this.filteredOffers[category].slice(start, start + this.cardsPerPage);
  }

  // Navigation helpers
  canNavigateLeft(category: CategoryKey): boolean {
    return this.pageIndex[category] > 0;
  }

  canNavigateRight(category: CategoryKey): boolean {
    return (this.pageIndex[category] + 1) * this.cardsPerPage < this.filteredOffers[category].length;
  }

  navigateLeft(category: CategoryKey): void {
    if (this.canNavigateLeft(category)) this.pageIndex[category]--;
  }

  navigateRight(category: CategoryKey): void {
    if (this.canNavigateRight(category)) this.pageIndex[category]++;
  }

  onOfferClick(offer: Offer): void {
    this.router.navigate(['/offer', offer.id]);
  }

  onViewBookings(): void {
    this.router.navigate(['/bookings']);
  }

  onCreateOffer(): void {
    this.showCreateOfferModal = true;
  }

  onLogout(): void {
    this.userService.logout().subscribe({
      next: () => this.finalizeLogout(),
      error: () => this.finalizeLogout()
    });
  }

  private finalizeLogout(): void {
    this.userService.removeToken();
    this.router.navigate(['/login']);
  }

  formatDate(dateString: string): string {
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

  getCategoryOffers(category: CategoryKey): Offer[] {
    return this.filteredOffers[category];
  }
}
