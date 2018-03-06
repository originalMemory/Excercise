class student(object):
    def __init__(self, name, score):
        self._name = name
        self._score = score

    def print_score(self):
        print('%s: %s' % (self._name, self._score))