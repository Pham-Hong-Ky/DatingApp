
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, CanDeactivateFn, RouterStateSnapshot, UrlTree } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { Inject, Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfirmService } from '../_services/confirm.service';



export const PreventUnsaveChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
  const confirmService = inject(ConfirmService);
  
  if(component.editForm.dirty){
      return confirmService.confirm()
    }
    return true;
  }

