import { NgFor } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm'	) messageForm: NgForm;
  @Input() messages: Message[]; 
  @Input() username: string;
  messageContent: string;

  constructor(public messagesService: MessageService ) { }
  
  ngOnInit(): void {
 
  }

  sendMessage(){
    this.messagesService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm.reset();
    })
  }

}
