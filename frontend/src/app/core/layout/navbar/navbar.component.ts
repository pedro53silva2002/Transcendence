import { AsyncPipe } from '@angular/common';
import { Component, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { debounceTime, delay, distinctUntilChanged, map, Observable, of, Subscription, tap } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatButtonModule,
			MatMenuModule,
			ReactiveFormsModule,
			MatFormField,
			MatInputModule,
			MatAutocompleteModule,
			RouterLink,
			RouterLinkActive,
			MatIconModule,
			MatSidenavModule,
			AsyncPipe],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit, OnDestroy {
	searchControl = new FormControl('');
	users = ['Maria', 'João', 'Diogo', 'Maria João', 'Rui Diogo', 'Rui'];
	filteredUsers: Observable<string[]> = new Observable();
	private searchSub?: Subscription;
	private backendSub?: Subscription;

	// constructor() {
	// 	this.searchControl.valueChanges.subscribe(value => {
	// 		console.log("Current search query:", value);
	// 	})
	// }

	ngOnInit(): void {
		this.searchSub = this.searchControl.valueChanges.pipe(
			debounceTime(300),
			distinctUntilChanged(), //if we type more and then change it to the previous input, it doesn't register any change
		).subscribe(abc => {
			//console.log(abc)
			 if(abc!= null){
			 	this.filteredUsers = this.getUsers(abc).pipe(
					tap(list => console.log(list)),
					map(list => list.map(name => name.toLowerCase()))
				);
			 }
		})
	}

	ngOnDestroy(): void {
		this.searchSub?.unsubscribe();
		this.backendSub?.unsubscribe();
	}

	//to close the side menu when the screen is resized to desktop size (if it's open)
	@ViewChild(MatMenuTrigger) menuTrigger!: MatMenuTrigger;

	@HostListener('window:resize', ['$event'])
	onResize(event: any) {
		if (window.innerWidth > 938 && this.menuTrigger && this.menuTrigger.menuOpen) {
		this.menuTrigger.closeMenu();
		}
	}


	private mockUsers = ['Alice Silva', 'Alice Costa','Alice Souza', 'Alice Lima'];

	  
	getUsers(text :string): Observable<string[]> {
		// 'of' transforma a S em um Observable
		// 'delay(1000)' pausa a emissão por 1 segundo (1000ms)
		return of(this.mockUsers.filter(nomeDeUser => nomeDeUser.toLowerCase().includes(text.toLowerCase()))).pipe(
		  delay(1000)
		);
	  }
}

