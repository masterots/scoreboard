import { ScoreboardAppPage } from './app.po';

describe('scoreboard-app App', () => {
  let page: ScoreboardAppPage;

  beforeEach(() => {
    page = new ScoreboardAppPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
