mergeInto(LibraryManager.library, {
  GameOverExtern: function (userName, score) {
    window.dispatchReactUnityEvent("GameOver", UTF8ToString(userName), score);
  },
});