|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|                                                     **Combination Lock Test Plan**                                                                                             |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|
|  **ID**  |                              **Tests**                                   |        **Sub-tests**          |               **Description**                 | **Date** |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|
|  **1**   |                  *Dial Locking and Rotation Tests*                       | *Rotatable Dial on Incomplete | Verify that the dial remains rotatable if:    |          |
|          |                                                                          |  Combination When Door is     |     - The combination is incomplete.          |          |
|          |                                                                          |  Open*                        |     - The door is open, but in an unopenable  |          |
|          |                                                                          |                               |       state.                                  |          |
|          |                                                                          |                               |                                               |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          |  *Dial Locks on Successful    | Confirm the dial locks, preventing            |          |
|          |                                                                          |   Combination Completion*     | further rotation when the combination         |          |
|          |                                                                          |                               | is successfully completed.                    |          |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|
|  **2**   |           *Door Locking and Combination Re-entry Requirement*            | *Door Remains Locked Until    | Ensure the door cannot be opened until        |          |
|          |                                                                          |   Combination is Complete*    | the entire combination sequence is            |          |
|          |                                                                          |                               | correctly entered.                            |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Door Requires Combination    | Verify that the door locks when closed,       |          |
|          |                                                                          |   Re-entry After Closing*     | requiring a full re-entry of the              |          |
|          |                                                                          |                               | combination to unlock again.                  |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Combination Check Disabled   | Confirm that the combination check logic      |          |
|          |                                                                          |   When Door is Open*          | disengages if the door is open.               |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Correct Lock State After     | Validate that the door can be locked or       |          |
|          |                                                                          |   Combination Completion and  | unlocked correctly based on the completion    |          |
|          |                                                                          |   Door Closing*               | of the combination and its open/close         |          |
|          |                                                                          |                               | status.                                       |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Combination Reset on Door    | Ensure that the combination resets            |          |
|          |                                                                          |   Relock*                     | entirely after the door is relocked.          |          |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|
|  **3**   |        *Combination Reset and Sequence Validation*                       | *Combination Resets on        | Validate that an incorrect input causes a     |          |
|          |                                                                          |   Incorrect Input*            | full reset of the combination if:             |          |
|          |                                                                          |                               |     - The rotation direction deviates.        |          |
|          |                                                                          |                               |     - The count or rotation pattern doesn’t   |          |
|          |                                                                          |                               |       align.                                  |          |
|          |                                                                          |                               |     - The dialed number is incorrect or fails |          |
|          |                                                                          |                               |       to match target conditions.             |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Manual Combination Reset     | Ensure that a manual reset resets all         |          |
|          |                                                                          |   Resets All Variables        | necessary variables, flags, and states        |          |
|          |                                                                          |   Correctly*                  | related to the combination.                   |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Manual Reset Function During | Verify that invoking a manual reset during    |          |
|          |                                                                          |   Combination Entry*          | combination entry clears the sequence and     |          |
|          |                                                                          |                               | resets the dial.                              |          |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|
|  **4**   |             *Combination and Sequence Tracking*                          | *Timer Limit Validation for   | Confirm that the number dialed is compared    |          |
|          |                                                                          |   Target Number Matching*     | to the target number once the timer           |          |
|          |                                                                          |                               | reaches a specific limit.                     |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Advancement Through          | Validate that the sequence advances only      |          |
|          |                                                                          |   Combination Sequence*       | under correct conditions:                     |          |
|          |                                                                          |                               |     - The target number and rotation match.   |          |
|          |                                                                          |                               |     - Timing and sequence conditions are met. |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Combination Validator Engaged| Ensure that the combination validator is      |          |
|          |                                                                          |   While Combination is        | engaged as long as the combination is         |          |
|          |                                                                          |   Incomplete*                 | incomplete.                                   |          |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|
|  **5**   |               *Door Position and Movement*                               | *Sound Cues for Door and      | Verify that appropriate sound cues play       |          |
|          |                                                                          |   Dial Movements*             | for door opening, closing, and dial           |          |
|          |                                                                          |                               | rotations.                                    |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Door Unlocks on Complete     | Ensure the door unlocks fully when the        |          |
|          |                                                                          |   Combination*                | combination sequence is correctly completed.  |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Door Rotation Limits         | Confirm the door’s rotation is limited        |          |
|          |                                                                          |   Enforced*                   | to 0 and 180 degrees, preventing over-        |          |
|          |                                                                          |                               | rotation.                                     |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Door Movement Matches        | Ensure door movement syncs with the speed     |          |
|          |                                                                          |   Controller Speed*           | of the user’s controller input.               |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Initial Door Position Set at | Check that the door’s initial position is     |          |
|          |                                                                          |   Start*                      | set correctly upon system start.              |          |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|
|  **6**   |             *Miscellaneous and State Tracking*                           | *Reset After Timeout*         | Validate that all combination states reset    |          |
|          |                                                                          |                               | if a timeout period is reached without        |          |
|          |                                                                          |                               | activity.                                     |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Track Predecessor Count for  | Ensure accurate tracking of previous states   |          |
|          |                                                                          |   State Management*           | or steps in the combination sequence.         |          |
|          |                                                                          |-------------------------------|-----------------------------------------------|----------|
|          |                                                                          | *Hand Position Validity During| Verify the user’s hand position and rotation  |          |
|          |                                                                          |   Rotation*                   | during dial rotation.                         |          |
|----------|--------------------------------------------------------------------------|-------------------------------|-----------------------------------------------|----------|

# Resources
- [Learn Git Branching](https://learngitbranching.js.org/)
- [Head First Git](https://www.oreilly.com/library/view/head-first-git/9781492092506/)
