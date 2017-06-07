import { NgappPage } from './app.po';

describe('ngapp App', () => {
  let page: NgappPage;

  beforeEach(() => {
    page = new NgappPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
