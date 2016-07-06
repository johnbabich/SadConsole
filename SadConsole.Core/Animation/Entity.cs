﻿//namespace SadConsole.Animation
//{
//    using Microsoft.Xna.Framework;
//    using Microsoft.Xna.Framework.Graphics;
//    using SadConsole.Consoles;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Runtime.Serialization;
//    using System;

//    public class AnimationRenderer : Dictionary<string, Animation>, IDraw
//    {
//        [DataMember(Name = "CurrentAnimation")]
//        private string currentAnimationName;

//        private Animation currentAnimation;
//        private Point position;


//        /// <summary>
//        /// Gets the current animation.
//        /// </summary>
//        public Animation CurrentAnimation {
//            get { return currentAnimation; }
//            set
//            {

//            }
//        }

//        /// <summary>
//        /// Gets or sets a point that is used to offset the position of the entity.
//        /// </summary>
//        public Point PositionOffset { get; set; }

//        /// <summary>
//        /// Gets or sets the position to render the cells.
//        /// </summary>
//        [DataMember]
//        public Point Position
//        {
//            get { return position; }
//            set { Point previousPosition = position; position = value; OnPositionChanged(previousPosition); }
//        }
        

//        public bool DoUpdate { get; set; }

//        public bool IsVisible { get; set; }           

//        public void Render()
//        {
//            if (IsVisible)
//            {
//                //draw
//            }
//        }

//        public void Update()
//        {
//            if (DoUpdate)
//            {
//                //update
//            }
//        }
        
//        public void SetCurrentAnimation(Animation animation)
//        {
//            if (ContainsValue(animation))
//                CurrentAnimation = animation;
//            else
//                throw new ArgumentException("Animation not found in this animation group.", "animation");
//        }

//        public void SetCurrentAnimation(string name)
//        {
//            if (ContainsKey(name))
//                CurrentAnimation = this[name];
//            else
//                throw new ArgumentException("Animation not found in this animation group.", "name");
//        }
//    }








//    /// <summary>
//    /// Represents a set of animations that can be positioned and rendered to the screen.
//    /// </summary>
//    [DataContract]
//    public class Entity: TextSurfaceRenderer, IDraw
//    {
//        #region Properties
//        /// <summary>
//        /// Gets the current animation's bounding box.
//        /// </summary>
//        public Rectangle AnimationBoundingBox { get { return _animationBoundingBox; } }

        

//        /// <summary>
//        /// Returns a read-only collection of all the animations this entity has.
//        /// </summary>
//        public System.Collections.ObjectModel.ReadOnlyCollection<Animation> Animations
//        {
//            get { return _animations.ToList().AsReadOnly(); }
//        }

        

//        public Font Font { get { return _font; } set { _font = Font; UpdateAnimationFont(); } }

//        /// <summary>
//        /// Called when the current animation state changes. Parameters are: current entity, current animation, new state, old state.
//        /// </summary>
//        public System.Action<Entity, Animation, AnimationState, AnimationState> OnEntityAnimationStateChanged;
//        private string fontName;
//        private Font.FontSizes fontSize;

//        /// <summary>
//        /// Indicates this entity should be drawn.
//        /// </summary>
//        public bool IsVisible { get; set; }

//        /// <summary>
//        /// Indicates this entity should update the animations.
//        /// </summary>
//        public bool DoUpdate { get; set; }

//        /// <summary>
//        /// Treats the <see cref="Position"/> of the console as if it is pixels and not cells.
//        /// </summary>
//        public bool UsePixelPositioning { get; set; } = false;

//        /// <summary>
//        /// The width of the entity.
//        /// </summary>
//        public int Width { get; protected set; }

//        /// <summary>
//        /// The height of the entity.
//        /// </summary>
//        public int Height { get; protected set; }

//        /// <summary>
//        /// A tint applied to the entity.
//        /// </summary>
//        public Color Tint { get; set; }
//        #endregion

//        #region Constructors
//        /// <summary>
//        /// Creates a new entity with the Engine's default font.
//        /// </summary>
//        /// <param name="width">The width of the entity. All animations should try and match this width.</param>
//        /// <param name="height">The height of the entity. All animations should try and match this height.</param>
//        public Entity(int width, int height) : this(width, height, Engine.DefaultFont) { }

//        /// <summary>
//        /// Creates a new entity with the specified font.
//        /// </summary>
//        /// <param name="width">The width of the entity. All animations should try and match this width.</param>
//        /// <param name="height">The height of the entity. All animations should try and match this height.</param>
//        /// <param name="font">The font to use when rendering this entity.</param>
//        public Entity(int width, int height, Font font)
//        {
//            _font = font;
//            IsVisible = true;
//            Width = width;
//            Height = height;

//            var defaultAnimation = new Animation("default", width, height);
//            defaultAnimation.CreateFrame();

//            AddAnimation(defaultAnimation);
//            SetCurrentAnimation(defaultAnimation);

//            _currentAnimation.Font = font;

//            CollisionBox = new Rectangle(0, 0, width, height);
//            _animationBoundingBox = new Rectangle(0, 0, width, height);
//            Tint = Color.Transparent;
//        }
//        #endregion

//        #region Methods

//        ///// <summary>
//        ///// Renders the entity using the specified sprite batch. This method will not call Batch.Begin and Batch.End.
//        ///// </summary>
//        ///// <param name="batch">The sprite batch to render the entity.</param>
//        //public void Render(SpriteBatch batch)
//        //{
//        //    var oldBatch = base.Batch;
//        //    base.Batch = batch;
//        //    base._skipBatchBeginEnd = true;
//        //    base.Render();
//        //    base.Batch = oldBatch;
//        //    base._skipBatchBeginEnd = false;
//        //}
        
//        public void Render()
//        {
//            if (IsVisible)
//                base.Render(_currentAnimation.CurrentFrame, _position, UsePixelPositioning);
//        }

//        ////TODO: This is not really working well now. Since positioning changed to be transform based, entity will always render 0,0 if you do not call begin and end.
//        ///// <summary>
//        ///// Renders the entity.
//        ///// </summary>
//        ///// <param name="batch">The sprite batched used for drawing the entity.</param>
//        //public void Render(SpriteBatch batch, bool callBeginEnd = false)
//        //{
//        //    if (_currentAnimation != null && !_currentAnimation.IsEmpty)
//        //    {
//        //        if (IsVisible)
//        //        {
//        //            Point worldLocation;

//        //            if (UseAbsolutePositioning)
//        //                worldLocation = Location;
//        //            else
//        //                worldLocation = Location.ConsoleLocationToWorld(_font.CellWidth, _font.CellHeight);

//        //            Matrix transform = Matrix.CreateTranslation(worldLocation.X, worldLocation.Y, 0f);

//        //            transform = transform * Matrix.CreateTranslation(-_currentAnimation.Center.X * _font.CellWidth, -_currentAnimation.Center.Y * _font.CellHeight, 0f);

//        //            if (callBeginEnd)
//        //                batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, null, transform);

//        //            Frame frame = _currentAnimation.CurrentFrame;

//        //            for (int cellIndex = 0; cellIndex < frame.CellCount; cellIndex++)
//        //            {
//        //                var cell = frame[cellIndex];

//        //                if (cell.IsVisible)
//        //                {
//        //                    if (cell.Effect != null && !cell.Effect.Finished)
//        //                        cell.Effect.Process(Engine.GameTimeElapsedUpdate, cell);

//        //                    // Draw the background of the cell as a solid color
//        //                    if (cell.ActualBackground != Color.Transparent)
//        //                        batch.Draw(Engine.BackgroundCell, frame.CellIndexRects[cellIndex], null, cell.ActualBackground, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);

//        //                    // "Print" the foreground of the cell on top of the background
//        //                    if (cell.ActualForeground != Color.Transparent)
//        //                        batch.Draw(Font.Image, frame.CellIndexRects[cellIndex], _font.CharacterIndexRects[cell.ActualCharacterIndex], cell.ActualForeground, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);

//        //                    // SHOULD HAVE THIS CELLINDEXRECTS I guess.. For position it shuold move all the rects around
//        //                }
//        //            }

//        //            if (callBeginEnd)
//        //                batch.End();
//        //        }
//        //    }
//        //}

//        /// <summary>
//        /// Called by the owning console during it's update method. Calls update the current animation.
//        /// </summary>
//        public virtual void Update()
//        {
//            if (DoUpdate)
//                if (_currentAnimation != null)
//                    _currentAnimation.Update();
//        }


//        /// <summary>
//        /// Stops the current animation from playing.
//        /// </summary>
//        public void Stop()
//        {
//            _currentAnimation.Stop();
//        }

//        /// <summary>
//        /// Starts the current animation.
//        /// </summary>
//        public void Start()
//        {
//            _currentAnimation.Start();
//        }

//        /// <summary>
//        /// Sets the active animation.
//        /// </summary>
//        /// <param name="name">The name of the animation to activate.</param>
//        public void SetActiveAnimation(string name)
//        {
//            var animation = _animations.Where(a => a.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

//            if (animation == null)
//                return;

//            SetCurrentAnimation(animation);
//            UpdateAnimationBoundingBox();
//        }

//        /// <summary>
//        /// Sets the active animation.
//        /// </summary>
//        /// <param name="name">The name of the animation to activate.</param>
//        /// <remarks>This animation does not have to be part of the named animations added to the entity.</remarks>
//        public void SetActiveAnimation(Animation animation)
//        {
//            SetCurrentAnimation(animation);
//            UpdateAnimationBoundingBox();
//        }

//        public void RemoveAllAnimations()
//        {
//            _animations.Clear();
//            SetCurrentAnimation(null);
//        }

//        /// <summary>
//        /// Updates the AnimationBoundingBox property with the values from the current animation. If you change the Center of the animation, this should be called.
//        /// <remarks>This is also called whenever the SetAnimation method is called.</remarks>
//        /// </summary>
//        public void UpdateAnimationBoundingBox()
//        {
//            _animationBoundingBox = new Rectangle(0 - _currentAnimation.Center.X, 0 - _currentAnimation.Center.Y, _currentAnimation.Width, _currentAnimation.Height);
//        }

//        /// <summary>
//        /// Returns an animation by name.
//        /// </summary>
//        /// <param name="name">The name of the animation.</param>
//        /// <returns>The animation if found, otherwise returns null.</returns>
//        public Animation GetAnimation(string name)
//        {
//            return _animations.Where(a => a.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
//        }

//        /// <summary>
//        /// Adds an animation to the entity. Replacing any animation with the same name and setting the font of the animation to the entity's font.
//        /// </summary>
//        /// <param name="animation">The animation.</param>
//        public void AddAnimation(Animation animation)
//        {
//            var oldAnimation = _animations.Where(a => a.Name.Equals(animation.Name, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

//            if (oldAnimation != null)
//                _animations.Remove(oldAnimation);
            
//            animation.Font = _font;

//            _animations.Add(animation);

//            if (_animations.Count == 1)
//                SetActiveAnimation(animation);
//        }

//        /// <summary>
//        /// Removes an animation from this entity.
//        /// </summary>
//        /// <param name="name">The name of an animation the entity has. If you try to remove the "default" animation, an exception will be thrown.</param>
//        public void RemoveAnimation(string name)
//        {
//            if (name == "default")
//                throw new System.Exception("Cannot remove default animation. Replace it by adding a new animation with the name 'default'");

//            var oldAnimation = _animations.Where(a => a.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

//            if (oldAnimation != null)
//            {
//                _animations.Remove(oldAnimation);

//                if (_currentAnimation == oldAnimation)
//                    SetActiveAnimation("default");
//            }
//        }

//        /// <summary>
//        /// Removes an animation from this entity.
//        /// </summary>
//        /// <param name="animation">The animation to remove. If you try to remove the "default" animation, an exception will be thrown.</param>
//        public void RemoveAnimation(Animation animation)
//        {
//            if (_animations.Contains(animation))
//            {
//                if (animation.Name == "default")
//                    throw new System.Exception("Cannot remove default animation. Replace it by adding a new animation with the name 'default'");
//                else
//                {
//                    _animations.Remove(animation);

//                    if (_currentAnimation == animation)
//                        SetActiveAnimation("default");
//                }
//            }
//        }

//        /// <summary>
//        /// Updates all existing animations with the entity's font.
//        /// </summary>
//        protected void UpdateAnimationFont()
//        {
//            for (int i = 0; i < _animations.Count; i++)
//                _animations[i].Font = _font;
//        }

//        protected void SetCurrentAnimation(Animation animation)
//        {
//            var oldAnimation = _currentAnimation;

//            if (oldAnimation != null)
//            {
//                oldAnimation.State = AnimationState.Deactivated;
//                oldAnimation.AnimationStateChanged -= CurrentAnimation_AnimationStateChanged;

//                oldAnimation.Restart();
//                oldAnimation.Stop();
//            }

//            _currentAnimation = animation;
            
//            if (_currentAnimation != null)
//            {
//                foreach (var frame in _currentAnimation.Frames)
//                    frame.Tint = Tint;

//                _currentAnimation.AnimationStateChanged += CurrentAnimation_AnimationStateChanged;
//                _currentAnimation.State = AnimationState.Activated;
//            }
//        }

//        private void CurrentAnimation_AnimationStateChanged(object sender, AnimationStateChangedEventArgs e)
//        {
//            OnEntityAnimationStateChanged?.Invoke(this, _currentAnimation, e.NewState, e.PreviousState);
//        }
//        #endregion

//        public override Matrix GetPositionTransform(Point position, Point CellSize, bool absolutePositioning)
//        {
//            Point worldLocation;

//            if (absolutePositioning)
//                worldLocation = position + PositionOffset;
//            else
//                worldLocation = position.ConsoleLocationToWorld(CellSize.X, CellSize.Y) + PositionOffset.ConsoleLocationToWorld(CellSize.X, CellSize.Y);

//            return Matrix.CreateTranslation(worldLocation.X, worldLocation.Y, 0f) * Matrix.CreateTranslation(-_currentAnimation.Center.X * _currentAnimation.Font.Size.X, -_currentAnimation.Center.Y * _currentAnimation.Font.Size.Y, 0f);
//        }

//        /// <summary>
//        /// Called when the <see cref="Position" /> property changes.
//        /// </summary>
//        /// <param name="oldLocation">The location before the change.</param>
//        protected virtual void OnPositionChanged(Point oldLocation) { }


//        public void Save(string file)
//        {
//            Serializer.Save(this, file, Serializer.EntityTypes);
//        }

//        public static Entity Load(string file)
//        {
//            return Serializer.Load<Entity>(file, Serializer.EntityTypes);
//        }

//        [OnSerializing]
//        private void BeforeSerializing(StreamingContext context)
//        {
//            fontName = Font.Name;
//            fontSize = Font.SizeMultiple;
//            _currentAnimationName = CurrentAnimation.Name;
//        }

//        [OnDeserialized]
//        private void AfterDeserialized(StreamingContext context)
//        {
//            Font font;

//            // Try to find font
//            if (Engine.Fonts.ContainsKey(fontName))
//                font = Engine.Fonts[fontName].GetFont(fontSize);
//            else
//                font = Engine.DefaultFont;

//            Font = font;
//            SetActiveAnimation(_currentAnimationName);

//            _currentAnimationName = null;
//            fontName = null;
//        }
//    }

    
//}