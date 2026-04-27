import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from './components/button/button.component';
import { CardComponent } from './components/card/card.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { ConfirmationPopUpComponent } from './pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule } from '@angular/material/card';
import { NavbarComponent } from '../core/layout/navbar/navbar.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, 
            MatButtonModule,
            MatCardModule,
            ConfirmationPopUpComponent,
            PageHeaderComponent,
            CardComponent,
            ButtonComponent,
            NavbarComponent],
  //components that are allowed to be used outside of this module
  exports: [ButtonComponent,
            CardComponent,
            PageHeaderComponent,
            ConfirmationPopUpComponent,
            MatButtonModule,
            MatCardModule,
            NavbarComponent],
})
export class SharedModule {}
