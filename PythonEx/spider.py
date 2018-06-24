import urllib.request, os, threading
from bs4 import BeautifulSoup

TUMBLR_NAME = 'makooooon'
PAGES = 80


def get_html(url):
    return BeautifulSoup(urllib.request.urlopen(url).read().decode(), "lxml")


class Download(threading.Thread):
    def __init__(self, imgs, page):
        self.start_page = page * 10
        self.imgs = imgs
        super().__init__()

    def run(self):
        for img in self.imgs:
            self.start_page += 1
            filename = 'imgs/' + self.start_page.__str__() + '.jpg'
            if os.path.exists(filename) and os.path.getsize(filename) > 0:
                continue
            with open(filename, 'wb') as f:
                f.write(urllib.request.urlopen(img).read())


class StartDownload(threading.Thread):
    def __init__(self, page):
        self.page = page
        super().__init__()

    def run(self):
        page = self.page
        html = get_html('http://' + TUMBLR_NAME + '.tumblr.com/page/' + page.__str__())
        imgs = html.select('.photo-post-photo')
        imgs = map(lambda i: i.attrs['src'].replace('_250', '_1280'), imgs)
        Download(list(imgs), page).start()
        print(page)


if __name__ == '__main__':
    page = 0
    while page < PAGES + 1:
        page += 1
        StartDownload(page).start()
