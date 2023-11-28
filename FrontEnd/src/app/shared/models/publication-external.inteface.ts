import { PostedComment } from "./comment.interface";
import { Publication } from "./publication.interface";

export interface PublicationExternal extends Publication {
  comments: Array<PostedComment>;
  likes: Array<string>;
  isLikedByCurrentUser: boolean;
}
